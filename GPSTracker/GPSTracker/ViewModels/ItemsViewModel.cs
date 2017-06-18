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
            Title = "Browse";
            Items = new ObservableRangeCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<ItemsPage, Position>(this, "LocationChanged", async (obj, item) =>
            {
                var _item = new Item
                {
                    Text = item.Timestamp.ToString("dd/MM hh:mm:ss"),
                    Description = item.Latitude + "-" + item.Longitude
                };
                Items.Add(_item);
                await DataStore.AddItemAsync(_item);
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