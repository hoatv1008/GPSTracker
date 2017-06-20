using System;
using System.Diagnostics;
using System.Threading.Tasks;

using GPSTracker.Helpers;
using GPSTracker.Models;
using GPSTracker.Views;

using Xamarin.Forms;
using Plugin.Geolocator.Abstractions;

namespace GPSTracker.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "Locations";
            Items = new ObservableRangeCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<Position>(this, "LocationChanged", (item) =>
            {
                var _item = new Item
                {
                    Text = item.Timestamp.ToString("dd/MM hh:mm:ss") + " " + item.Latitude + ":" + item.Longitude,
					Description = $"Heading: {item.Heading.ToString()}"
								+ Environment.NewLine + $"Speed: {item.Speed.ToString()}"
								+ Environment.NewLine + $"Accuracy: {item.Accuracy.ToString()}"
								+ Environment.NewLine + $"Altitude: {item.Altitude.ToString()}"
								+ Environment.NewLine + $"AltitudeAccuracy: {item.AltitudeAccuracy.ToString()}",
                    Longitude = item.Longitude,
                    Latitude = item.Latitude
                };
                Items.Insert(0, _item);

                try
                {
                    for (var i = Items.Count; i > 1000; i--)
                        Items.RemoveAt(i);
                }
                catch
                {
                    // donothing
                }

            });

            MessagingCenter.Subscribe<ClearLocationMessage>(this, "ClearAllLocations", (obj) => {
                Items.Clear();
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                Items.ReplaceRange(items);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load items.",
                    Cancel = "OK"
                }, "message");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}