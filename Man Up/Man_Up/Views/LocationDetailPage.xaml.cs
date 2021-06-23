using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Man_Up.ViewModels;
using Man_Up.Views;
using Xamarin.Forms.Maps;
using Man_Up.Models;
using System.Diagnostics;
using Plugin.Geolocator;
using Plugin.Messaging;

namespace Man_Up.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocationDetailPage : ContentPage
    {
        LocationDetailViewModel viewModel;
        Geocoder geoCoder;

        public LocationDetailPage()
        {
            InitializeComponent();
        }

        public LocationDetailPage(LocationDetailViewModel viewModel)
        {

            InitializeComponent();

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    svMain.InputTransparent = false;
                    break;
                case Device.Android:
                    svMain.InputTransparent = true;
                    break;
            }

            //callto:12345+type=phone

            phone.GestureRecognizers.Add(new TapGestureRecognizer((view) => OnLabelClicked()));

            //svMain.InputTransparent = Device.OnPlatform(false, true, false);
            BindingContext = this.viewModel = viewModel;
            ResetMap();
            IntitializeButtons();
        }

        private void OnLabelClicked()
        {
            /*var phoneDialer = CrossMessaging.Current.PhoneDialer;
            if (phoneDialer.CanMakePhoneCall)
                phoneDialer.MakePhoneCall("+272193343499");*/
            Device.OpenUri(new Uri("tel:" + phone.Text));
        }

        /// <summary>
        /// Method in Development Mode (Currently Beta)
        /// </summary>
        private async void GetGeoCode()
        {
            var address = "11B Pateke Place";
            var approximateLocations = await geoCoder.GetPositionsForAddressAsync(address);
            string output = "";
            foreach (var position in approximateLocations)
            {
                output += position.Latitude + ", " + position.Longitude + "\n";
            }
        }

        /// <summary>
        /// Sets up extra buttons for directions, resetting map and top top of page
        /// </summary>
        private void IntitializeButtons()
        {
            Button btnGetDirections = new Button();
            btnGetDirections.Text = "Get Directions";
            btnGetDirections.Clicked += BtnGetDirections_Clicked;
            GridMapButtons.Children.Add(btnGetDirections, 0, 0);

            Button btnResetMap = new Button();
            btnResetMap.Text = "Reset Map";
            btnResetMap.Clicked += BtnResetMap_Clicked;
            GridMapButtons.Children.Add(btnResetMap, 1, 0);

            Button btnTop = new Button();
            btnTop.Text = "To Top";
            btnTop.Clicked += BtnTop_Clicked;
            slMain.Children.Add(btnTop);
        }

        /// <summary>
        /// Button Click event to reset map back to default location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnResetMap_Clicked(object sender, EventArgs e)
        {
            ResetMap();
        }

        /// <summary>
        /// Method in Development Mode (Currently Beta)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnGetDirections_Clicked(object sender, EventArgs e)
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(5));
            if (position.Latitude != 0 && position.Longitude != 0)
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        iOSOpenMapsApp(position);
                        break;
                    case Device.Android:
                        AndroidOpenMapsApp(position);
                        break;
                }
            }
        }

        private void iOSOpenMapsApp(Plugin.Geolocator.Abstractions.Position position)
        {
            string url = "http://maps.apple.com/?saddr=" + position.Latitude + "," + position.Longitude + "&daddr=" + viewModel.Item.Coords[0] + "," + viewModel.Item.Coords[1];
            url = url.Replace(" ", "%20");
            Device.OpenUri(new Uri(url));
        }

        private void AndroidOpenMapsApp(Plugin.Geolocator.Abstractions.Position position)
        {
            try
            {
                //string request = string.Format("google.navigation:q=" + viewModel.Item.Coords[0] + "," + viewModel.Item.Coords[1]);
                string request = @"http://maps.google.com/maps?saddr="+ position.Latitude
                +"," + position.Longitude
                + "&daddr="
                + viewModel.Item.Coords[0] + "," + viewModel.Item.Coords[1];
                Device.OpenUri(new Uri(request));
            }
            catch (Exception ex)
            {
                string exmessage = ex.Message;
            }
        }

        private void ResetMap()
        {
            if (viewModel.Item.Coords.Length == 2)
            {
                geoCoder = new Geocoder();

                //GetGeoCode();

                var zoomLevel = 16; // pick a value between 1 and 18
                var latlongdeg = 360 / (Math.Pow(2, zoomLevel));

                MyMap.MoveToRegion(new MapSpan(new Position(viewModel.Item.Coords[0], viewModel.Item.Coords[1]), latlongdeg, latlongdeg)); // Santa Cruz golf course
                var position = new Position(viewModel.Item.Coords[0], viewModel.Item.Coords[1]); // Latitude, Longitude
                var pin = new Pin
                {
                    Type = PinType.Place,
                    Position = position,
                    Label = this.viewModel.Item.MapLocationName,
                    Address = this.viewModel.Item.MapAddress,
                };
                MyMap.MapType = MapType.Street;
                MyMap.Pins.Add(pin);
            }
            else
            {
                Label lblError = new Label();
                lblError.Text = Environment.NewLine + "No Map Data Exists for Location" + Environment.NewLine;
                lblError.FontAttributes = FontAttributes.Bold;
                lblError.FontSize = 20;
                slMain.Children.Add(lblError);
            }
        }

        private void BtnTop_Clicked(object sender, EventArgs e)
        {
            ScrollUp();
        }

        public async void ScrollUp()
        {
            await svMain.ScrollToAsync(0, 0, true);
        }
    }
}