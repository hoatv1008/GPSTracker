using GPSTracker.Models;
using GPSTracker.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace GPSTracker
{
	public partial class App : Application
	{
        public App()
		{
			InitializeComponent();

			SetMainPage();
		}

		public static void SetMainPage()
		{
            Current.MainPage = new TabbedPage
            {
                Children =
                {
					new NavigationPage(new AboutPage())
					{
						Title = "Maps",
						Icon = Device.OnPlatform<string>("tab_feed.png",null,null)
					},
                    new NavigationPage(new ItemsPage())
                    {
                        Title = "Locations",
                        Icon = Device.OnPlatform<string>("tab_about.png",null,null)
                    }
                }
            };

        }

        protected override void OnStart()
        {
            base.OnStart();

            MessagingCenter.Send(new StartListenLocationMessage(), "StartListenLocationChanged");
        }

	}
}
