using System;
using System.Threading;
using System.Threading.Tasks;
using GPSTracker.Models;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using UIKit;
using Xamarin.Forms;

namespace GPSTracker.Services
{
    public class LocationService
    {
        Position lastKnowPosition;
		nint _taskId;
		CancellationTokenSource _cts;

        public LocationService()
        {
            CrossGeolocator.Current.PositionChanged += Current_PositionChanged;
			CrossGeolocator.Current.AllowsBackgroundUpdates = true;
			CrossGeolocator.Current.PausesLocationUpdatesAutomatically = true;
			CrossGeolocator.Current.DesiredAccuracy = 2;
        }

        private async void OnExpiration() {
            await StopListening();
        }


        public async Task StartListening()
		{
			_cts = new CancellationTokenSource();

			_taskId = UIApplication.SharedApplication.BeginBackgroundTask("LongRunningTask", OnExpiration);

            await CrossGeolocator.Current.StartListeningAsync(5, 5, false);


            if (!CrossGeolocator.Current.IsGeolocationAvailable)
                MessagingCenter.Send(new LocationServiceNotAvailableMessage(), "LocationServiceNotAvailableMessage");

            if (!CrossGeolocator.Current.IsGeolocationEnabled)
				MessagingCenter.Send(new LocationServiceNotEnableMessage(), "LocationServiceNotEnableMessage");
            
			try
			{
                do
                {
                    await Task.Delay(10000, _cts.Token);

                    _cts.Token.ThrowIfCancellationRequested();

                } while (true);
			}
			catch (OperationCanceledException)
			{
			}
			finally
			{
				Console.WriteLine("Cancelled");
			}

			UIApplication.SharedApplication.EndBackgroundTask(_taskId);
		}

		public async Task StopListening()
		{
            _cts.Cancel();
			await CrossGeolocator.Current.StopListeningAsync();
		}

		private void Current_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
		{
			if (lastKnowPosition == null || lastKnowPosition.Latitude != e.Position.Latitude || lastKnowPosition.Longitude != e.Position.Longitude)
			{
				lastKnowPosition = e.Position;

				Device.BeginInvokeOnMainThread(() =>
				{
					MessagingCenter.Send(e.Position, "LocationChanged");
				});
			}
		}
    }
}
