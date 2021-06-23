using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Man_Up.Models;

using Xamarin.Forms;

[assembly: Dependency(typeof(Man_Up.Services.LocationDataStore))]
namespace Man_Up.Services
{
    public class LocationDataStore : IDataStore<Location>
    {
        bool isInitialized;
        List<Location> locations;

        public async Task<bool> AddItemAsync(Location location)
        {
            await InitializeAsync();

            locations.Add(location);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Location location)
        {
            await InitializeAsync();

            var _location = locations.Where((Location arg) => arg.Id == location.Id).FirstOrDefault();
            locations.Remove(_location);
            locations.Add(location);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(Location location)
        {
            await InitializeAsync();

            var _location = locations.Where((Location arg) => arg.Id == location.Id).FirstOrDefault();
            locations.Remove(_location);

            return await Task.FromResult(true);
        }

        public async Task<Location> GetItemAsync(string id)
        {
            await InitializeAsync();

            return await Task.FromResult(locations.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Location>> GetItemsAsync(bool forceRefresh = false)
        {
            await InitializeAsync();

            return await Task.FromResult(locations);
        }

        public Task<bool> PullLatestAsync()
        {
            return Task.FromResult(true);
        }


        public Task<bool> SyncAsync()
        {
            return Task.FromResult(true);
        }

        public async Task InitializeAsync()
        {
            if (isInitialized)
                return;

            locations = new List<Location>();
            var _locations = new List<Location>
            {
                new Location { Id = Guid.NewGuid().ToString(), LocationName = "Aranui", AddressLine1 = "123 Hampshire Street", AddressLine2 = "", DayOfWeek = "Wednesday", Time= "7:00pm", Facilitator = "William Mamaku", FacilitatorPhone = "027 923 8608", Coords = new double[] { -43.508103, 172.705493 }, MapLocationName = "123 Hampshire Street", MapAddress= "Aranui" },
                new Location { Id = Guid.NewGuid().ToString(), LocationName = "Ashburton", AddressLine1 = "McDonalds Ashburton", AddressLine2 = "Cnr West & Moore Streets", DayOfWeek = "Tuesday", Time= "7:30pm", Facilitator = "Regan Davis", FacilitatorPhone = "021 023 3635", Coords = new double[] { -43.905222, 171.744910 }, MapLocationName = "McDonalds Ashburton", MapAddress= "Cnr West & Moore Streets, Ashburton" },
                //new Location { Id = Guid.NewGuid().ToString(), LocationName = "Man Up Hornby", AddressLine1 = "McDonalds Hornby", AddressLine2 = "Main South Road", DayOfWeek = "Tuesday", Time= "7:00pm", Facilitator = "Mike Garth", FacilitatorPhone = "021 359 894", Coords = new double[] { -43.543408, 172.522042 }, MapLocationName = "McDonalds Hornby", MapAddress= "Main South Road, Hornby" },
                new Location { Id = Guid.NewGuid().ToString(), LocationName = "New Brighton", AddressLine1 = "North New Brighton Community Centre", AddressLine2 = "88 Marine Parade", DayOfWeek = "Wednesday", Time= "7:00pm", Facilitator = "Ben Robertson", FacilitatorPhone = "027 473 7149", Coords = new double[] { -43.494941, 172.726506 }, MapLocationName = "North New Brighton Community Centre", MapAddress= "88 Marine Parade, New Brighton" },
                new Location { Id = Guid.NewGuid().ToString(), LocationName = "Phillipstown", AddressLine1 = "Phillipstown Community Hub", AddressLine2 = "39 Nursery Road", DayOfWeek = "Wednesday", Time= "7:00pm", Facilitator = "Kanohi Vercoe", FacilitatorPhone = "027 509 8761", Coords = new double[] { -43.537866, 172.656557 }, MapLocationName = "Phillipstown Community Hub", MapAddress= "39 Nursery Road" },
                new Location { Id = Guid.NewGuid().ToString(), LocationName = "Rangiora", AddressLine1 = "Rangiora War Memorial Building", AddressLine2 = "24 High Street", DayOfWeek = "Wednesday", Time= "7:00pm", Facilitator = "Zane Tait", FacilitatorPhone = "022 044 2945", Coords = new double[] { -43.302965, 172.597580 }, MapLocationName = "Rangiora War Memorial Building", MapAddress= "24 High Street" },
                new Location { Id = Guid.NewGuid().ToString(), LocationName = "Shirley", AddressLine1 = "McFarlane Park Community Centre", AddressLine2 = "17 Acheson Ave", DayOfWeek = "Tuesday", Time= "7:00pm", Facilitator = "Phillip Wells", FacilitatorPhone = "0204 015 1245", Coords = new double[] { -43.500827, 172.654089 }, MapLocationName = "McFarlane Park Community Centre", MapAddress= "17 Acheson Ave" },
                new Location { Id = Guid.NewGuid().ToString(), LocationName = "St Albans", AddressLine1 = "St Albans Community Centre", AddressLine2 = "1047 Colombo St", DayOfWeek = "Wednesday", Time= "7:00pm", Facilitator = "Liam Godfery", FacilitatorPhone = "021 262 3318", Coords = new double[] { -43.514670, 172.636327 }, MapLocationName = "St Albans Community Centre", MapAddress= "1047 Colombo St" },
                new Location { Id = Guid.NewGuid().ToString(), LocationName = "Sydenham", AddressLine1 = "Derricks Panel & Paint", AddressLine2 = "Cnr Coleridge & Gasson St", DayOfWeek = "Monday", Time= "6:30pm", Facilitator = "Derek Tait", FacilitatorPhone = "021 950 064", Coords = new double[] { -43.545925, 172.642413 }, MapLocationName = "Derricks Panel & Paint", MapAddress= "Cnr Coleridge & Gasson St" },
            };

            foreach (Location location in _locations)
            {
                locations.Add(location);
            }

            isInitialized = true;
        }
    }
}
