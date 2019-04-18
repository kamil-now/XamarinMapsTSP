using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
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
        });
        public static LocationList GetMockData(IGeolocationService geolocation)
        {
            var list = new LocationList()
            {
                Locations = new ObservableCollection<Location>() //max 10
                {
                    new Location() { Position = new Xamarin.Forms.Maps.Position(50.252711, 19.015991) },
                    new Location() { Position = new Xamarin.Forms.Maps.Position(50.255272, 19.035315) },
                    new Location() { Position = new Xamarin.Forms.Maps.Position(50.261930, 19.008051) },
                    new Location() { Position = new Xamarin.Forms.Maps.Position(50.241313, 19.017488) },
                    new Location() { Position = new Xamarin.Forms.Maps.Position(50.247246, 18.997965) },
                    new Location() { Position = new Xamarin.Forms.Maps.Position(50.247866, 19.025751) },
                    new Location() { Position = new Xamarin.Forms.Maps.Position(50.254237, 19.019152) },
                    new Location() { Position = new Xamarin.Forms.Maps.Position(50.242092, 19.028267) },
                    new Location() { Position = new Xamarin.Forms.Maps.Position(50.254785, 19.004781) },
                    new Location() { Position = new Xamarin.Forms.Maps.Position(50.269412, 19.035555) }
                }
            };
            list.Locations.ForEach(x => x = geolocation.GetLocationList(x.Position).FirstOrDefault());
            list.NotifyOfPropertyChange(() => list.Locations);
            return list;
        }
    }
}
