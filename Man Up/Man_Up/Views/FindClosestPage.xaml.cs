using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Man_Up.Models;
using Man_Up.ViewModels;
using Plugin.Geolocator;
using Man_Up.Helpers;
using System.Windows.Input;

namespace Man_Up.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FindClosestPage : ContentPage
    {
        FindClosestViewModel viewModel;
        bool DisplayedLocationError = false;

        public FindClosestPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new FindClosestViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var location = args.SelectedItem as Location;
            if (location == null)
                return;

            await Navigation.PushAsync(new LocationDetailPage(new LocationDetailViewModel(location)));

            // Manually deselect item
            ItemsListView.SelectedItem = null;
        }

        async void FindClosestLocations()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            Plugin.Geolocator.Abstractions.Position position = new Plugin.Geolocator.Abstractions.Position();
            try
            {
                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(5));
            }
            catch (Exception ex)
            {
                if (!DisplayedLocationError)
                {
                    DisplayedLocationError = true;
                    await DisplayAlert("Location Services Not Enabled", "Please Enable Location Services in order to get the most out of this app", "OK");
                    position = new Plugin.Geolocator.Abstractions.Position();
                }
            }

            try
            {
                foreach (Location l in this.viewModel.Locations)
                {
                    if (position.Latitude != 0 && position.Longitude != 0)
                    {
                        double distance = DistanceTo(new double[] { position.Latitude, position.Longitude }, l.Coords, 'K');
                        l.Distance = distance;
                        l.DistanceString = Math.Round(distance, 2).ToString() + " km Away";
                    }
                    else
                    {
                        l.DistanceString = "Trying to get Distance";
                    }
                }
                ICommand command =  viewModel.RefreshCommand;
                command.Execute(this);
            }
            catch (Exception ex)
            {
                string exMessage = ex.Message;
            }
        }

        public static double DistanceTo(double[] location1, double[] location2, char unit = 'K')
        {
            double rlat1 = Math.PI * location1[0] / 180;
            double rlat2 = Math.PI * location2[0] / 180;
            double theta = location1[1] - location2[1];
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            switch (unit)
            {
                case 'K': //Kilometers -> default
                    return dist * 1.609344;
                case 'N': //Nautical Miles 
                    return dist * 0.8684;
                case 'M': //Miles
                    return dist;
            }

            return dist;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Locations.Count == 0)
            {
                viewModel.LoadItemsCommand.Execute(null);
                FindClosestLocations();
            }

        }
    }
}