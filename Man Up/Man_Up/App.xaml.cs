using Man_Up.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Man_Up
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
                    new NavigationPage(new HomePage())
                    {
                        Title = Xamarin.Forms.Device.OnPlatform("Home","Home","Home"),
                        Icon = Xamarin.Forms.Device.OnPlatform("tab_about.png","tab_home.png",null)
                    },
                    new NavigationPage(new LocationsPage())
                    {
                        Title = Xamarin.Forms.Device.OnPlatform("Groups","Groups","Groups"),
                        Icon = Xamarin.Forms.Device.OnPlatform("tab_feed.png","tab_groups.png",null)
                    },
                    new NavigationPage(new FindClosestPage())
                    {
                        Title = Xamarin.Forms.Device.OnPlatform("Find Closest","Find Closest","Find Closest"),
                        Icon = Xamarin.Forms.Device.OnPlatform("tab_feed.png","tab_find_closest.png",null)
                    },
                    new NavigationPage(new ContactUsPage())
                    {
                        Title = Xamarin.Forms.Device.OnPlatform("Contact","Contact","Contact"),
                        Icon = Xamarin.Forms.Device.OnPlatform("tab_contact.png","tab_contact.png",null)
                    },
                }
            };
        }

        protected override void OnStart()
        {
            MobileCenter.Start("android=8e3b8510-2d0f-4b9f-9753-757829137a9b; " +
                "ios=084b44a8-fabb-4de8-a808-6308cef5871d;",
                typeof(Analytics), typeof(Crashes));
            base.OnStart();
        }
    }
}
