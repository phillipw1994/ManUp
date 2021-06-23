namespace Xamarin_Test.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            //Xamarin.FormsMaps.Init("INSERT_AUTHENTICATION_TOKEN_HERE");
            LoadApplication(new Xamarin_Test.App());
        }
    }
}