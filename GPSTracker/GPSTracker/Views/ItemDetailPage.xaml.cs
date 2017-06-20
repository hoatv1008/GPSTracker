
using GPSTracker.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace GPSTracker.Views
{
	public partial class ItemDetailPage : ContentPage
	{
		ItemDetailViewModel viewModel;

        // Note - The Xamarin.Forms Previewer requires a default, parameterless constructor to render a page.
        public ItemDetailPage()
        {
            InitializeComponent();
        }

        public ItemDetailPage(ItemDetailViewModel viewModel)
		{
			InitializeComponent();

			BindingContext = this.viewModel = viewModel;

            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(viewModel.Item.Latitude, viewModel.Item.Longitude), Distance.FromMeters(100)));
			var pin = new Pin
			{
				Type = PinType.Place,
				Position = new Position(viewModel.Item.Latitude, viewModel.Item.Longitude),
				Label = "Current Position",
				Address = "custom detail info"
			};
			MyMap.Pins.Add(pin);
		}
	}
}
