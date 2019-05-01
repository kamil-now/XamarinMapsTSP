using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinTSP.Common.Abstractions;
using XamarinTSP.UI.Abstractions;

namespace XamarinTSP.UI.Models
{
    public class LocationList : PropertyChangedBase
    {
        public Location SelectedLocation { get; set; }
        public ObservableCollection<Location> Locations { get; set; }

        public LocationList()
        { 
            Locations = new ObservableCollection<Location>();
            Locations.CollectionChanged += (s, e) => NotifyOfPropertyChange();
        }
        public ICommand DeleteCommand => new Command<Location>(location =>
        {
            Locations.Remove(location);
            location.Dispose();
        });
        
        public void SetMockData(IGeolocationService geolocation)
        {
            Locations.Clear();
            void addLocation(double latitude, double longitude)
            {
                Locations.Add(new Location(geolocation.GetAddressList(new Xamarin.Forms.Maps.Position(latitude, longitude)).FirstOrDefault()));
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
            addLocation(50.235788, 18.978714);
            addLocation(50.266317, 18.995626);
            addLocation(50.268255, 19.019297);
        }
    }
}
