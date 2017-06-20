using System;

using GPSTracker.Models;
using GPSTracker.ViewModels;

using Xamarin.Forms;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace GPSTracker.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;


        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();
		}

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item
            ItemsListView.SelectedItem = null;
        }

        async void GPS_Clicked(object sender, EventArgs e)
        {
            var button = (ToolbarItem)sender;
            if (CrossGeolocator.Current.IsListening == false)
            {
                MessagingCenter.Send(new StartListenLocationMessage(), "StartListenLocationChanged");
                button.Text = "Stop";
            }
            else
            {
                MessagingCenter.Send(new StopListenLocationMessage(), "StopListenLocationChanged");
                button.Text = "Start";
            }
            await Task.FromResult(true);
        }

        async void Clear_Clicked(object sender, EventArgs e){

            MessagingCenter.Send(new ClearLocationMessage(), "ClearAllLocations");

            await Task.FromResult(true);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);

            if (CrossGeolocator.Current.IsListening)
			{
				btnGPS.Text = "Stop";
			}
			else
			{
				btnGPS.Text = "Start";
			}
        }


    }
}
