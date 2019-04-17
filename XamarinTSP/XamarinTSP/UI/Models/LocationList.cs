using System.Collections.ObjectModel;
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
        }

        public ICommand DeleteCommand => new Command<Location>(location =>
        {
            Locations.Remove(location);
        });
    }
}
