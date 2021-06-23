using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Man_Up.Helpers;
using Man_Up.Models;
using Man_Up.Views;

using Xamarin.Forms;
using System.Windows.Input;
using Plugin.Geolocator;
using System.Linq;

namespace Man_Up.ViewModels
{
    class FindClosestViewModel : LocationViewModel
    {
        public ObservableRangeCollection<Location> Locations { get; set; }
        public Command LoadItemsCommand { get; set; }

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public FindClosestViewModel()
        {
            Title = "Find Closest";
            Locations = new ObservableRangeCollection<Location>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;
                    await FindClosestLocations();
                    await ExecuteLoadItemsCommand();
                    IsRefreshing = false;
                });
            }
        }


        async Task<bool> FindClosestLocations()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            bool positionReceived = false;
            Plugin.Geolocator.Abstractions.Position position;
            try
            {
                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(5));
                positionReceived = true;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Location Services Not Enabled", "Please Enable Location Services in order to get the most out of this app", "OK");
                position = new Plugin.Geolocator.Abstractions.Position();
                positionReceived = false;
            }
            try
            {

                foreach (Location l in this.Locations)
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

                return true;
            }
            catch (Exception ex)
            {
                string exMessage = ex.Message;
                return false;
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

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Locations.Clear();
                var locations = await LocationDataStore.GetItemsAsync(true);
                Locations.ReplaceRange(locations.OrderBy(loc => loc.Distance));
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
