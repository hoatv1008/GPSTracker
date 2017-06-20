using System;
using System.Linq;
using System.Threading.Tasks;
using GPSTracker.Models;
using GPSTracker.ViewModels;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace GPSTracker.Views
{
    public partial class AboutPage : ContentPage
    {
        AboutViewModel viewModel;

        public AboutPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new AboutViewModel();

            viewModel.Items.CollectionChanged += async (sender, e) =>
            {
                await ShowMap();
            };


        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
            
        }

        protected async Task ShowMap()
        {
            Position center;

            if (viewModel.Items.Count == 0)
            {
                var current = await CrossGeolocator.Current.GetPositionAsync(2000);
                center = new Position(current.Latitude, current.Longitude);
            }
            else
            {
                var first = viewModel.Items.FirstOrDefault();
                center = new Position(first.Latitude, first.Longitude);
            }


            FullMap.MoveToRegion(MapSpan.FromCenterAndRadius(center, Distance.FromMeters(100)));
            FullMap.Pins.Clear();

			foreach (var item in viewModel.Items)
			{
				var pin = new Pin
				{
                    Type = PinType.SearchResult,
					Position = new Position(item.Latitude, item.Longitude),
					Label = item.Text,
					Address = item.Description
				};
				FullMap.Pins.Add(pin);



			}

            Console.WriteLine(viewModel.Items.Count());
        }
    }
}
