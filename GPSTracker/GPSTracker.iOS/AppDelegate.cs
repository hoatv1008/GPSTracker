
using System;
using Foundation;
using GPSTracker.Models;
using GPSTracker.Services;
using Plugin.Geolocator.Abstractions;
using UIKit;
using Xamarin.Forms;

namespace GPSTracker.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		/// <summary>
		/// Get the azure service instance
		/// </summary>
		public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();
        private LocationService locService = new LocationService();

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();
            global::Xamarin.FormsMaps.Init();

			LoadApplication(new App());

			MessagingCenter.Subscribe<Position>(this, "LocationChanged", async (item) =>
			{
				var _item = new Item
				{
					Text = item.Timestamp.ToString("dd/MM hh:mm:ss") + " Lat-Lon: " + item.Latitude + "-" + item.Longitude,
					Description = $"Heading: {item.Heading.ToString()}"
								+ Environment.NewLine + $"Speed: {item.Speed.ToString()}"
								+ Environment.NewLine + $"Accuracy: {item.Accuracy.ToString()}"
								+ Environment.NewLine + $"Altitude: {item.Altitude.ToString()}"
								+ Environment.NewLine + $"AltitudeAccuracy: {item.AltitudeAccuracy.ToString()}",
					Longitude = item.Longitude,
					Latitude = item.Latitude
				};
				
				await DataStore.AddItemAsync(_item);
			});

			MessagingCenter.Subscribe<ClearLocationMessage>(this, "ClearAllLocations", async (obj) =>
			{
				await DataStore.ClearItemAsync();
			});

			MessagingCenter.Subscribe<StartListenLocationMessage>(this, "StartListenLocationChanged", async message =>
			{   
                await locService.StartListening();
			});

            MessagingCenter.Subscribe<StopListenLocationMessage>(this, "StopListenLocationChanged", async message =>
			{
                await locService.StopListening();
			});

            MessagingCenter.Subscribe<LocationServiceNotEnableMessage>(this, "LocationServiceNotEnableMessage", message =>
			{
				
			});

			MessagingCenter.Subscribe<LocationServiceNotAvailableMessage>(this, "LocationServiceNotAvailableMessage", message =>
			{
				
			});

			return base.FinishedLaunching(app, options);
		}
	}
}
