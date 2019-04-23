using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinTSP.Abstractions;

namespace XamarinTSP.Utilities
{
    public class LocationList : PropertyChangedBase
    {
        public Location SelectedLocation { get; set; }
        public ObservableCollection<Location> Locations { get; set; }

        public LocationList()
        {
            Locations = new ObservableCollection<Location>();
            Locations.CollectionChanged += (s, e) => NotifyOfPropertyChange(() => Locations);
        }
        public ICommand DeleteCommand => new Command<Location>(location =>
        {
            Locations.Remove(location);
            location.Dispose();
        });
        public void SetMockData(IGeolocationService geolocation)
        {
            void addLocation(double latitude, double longitude)
            {
                Locations.Add(geolocation.GetLocationList(new Xamarin.Forms.Maps.Position(latitude, longitude)).FirstOrDefault());
            }
            addLocation(50.252711, 19.015991);
            addLocation(50.255272, 19.035315);
            addLocation(50.261930, 19.008051);
            addLocation(50.241313, 19.017488);
            addLocation(50.247246, 18.997965);
            addLocation(50.247866, 19.025751);
            addLocation(50.254237, 19.019152);
            addLocation(50.242092, 19.028267);
            addLocation(50.254785, 19.004781);
            addLocation(50.269412, 19.035555);
        }
    }
}
