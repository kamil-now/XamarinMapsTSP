using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinTSP.Abstractions;
using XamarinTSP.Utilities;

namespace XamarinTSP.UI.ViewModels
{
    public class LocationListViewModel : PropertyChangedBase
    {
        public LocationListViewModel()
        {
            Locations = new ObservableCollection<Location>() { new Location() { Name = "" } };
        }

        public ObservableCollection<Location> Locations { get; set; }
        public Location SelectedLocation { get; set; }
        public Location NewLocation { get; set; }
        public ICommand SelectCommand => new Command(() =>
        {

        });
        public ICommand AddLocationCommand => new Command(() =>
        {
            Locations.Add(new Location() { Name = "" });
        });
        public ICommand DeleteCommand => new Command<Location>(location =>
        {
            Locations.Remove(location);
        });
    }
}
