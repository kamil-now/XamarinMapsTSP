using System.Collections.ObjectModel;
using XamarinTSP.Abstractions;

namespace XamarinTSP.Utilities
{
    public class LocationList : PropertyChangedBase
    {
        public ObservableCollection<Location> Locations { get; set; }
        public LocationList()
        {
            Locations = new ObservableCollection<Location>();
        }
    }
}
