using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinTSP.Common.Abstractions;
using XamarinTSP.UI.Abstractions;
using XamarinTSP.UI.Models;

namespace XamarinTSP.UI.ViewModels
{
    public class LocationListViewModel : ViewModelBase
    {
        private IGeolocationService _geolocationService;
        private INavigator _navigator;
        private LocationList _list;
        private string _searchString;
        public ObservableCollection<Location> FoundLocations { get; set; }

        public string SearchString
        {
            get => _searchString;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _searchString = value;
                    App.InvokeOnMainThreadAsync(async () =>
                    {
                        var result = await _geolocationService.GetAddressListAsync(value);
                        FoundLocations = new ObservableCollection<Location>(result.Select(address => new Location(FoundLocations.Count + 1, address)));
                        NotifyOfPropertyChange(() => FoundLocations);
                    }, 100);
                }
            }
        }
        public LocationListViewModel(LocationList list, INavigator navigator)
        {
            _list = list;
            _navigator = navigator;
            _geolocationService = DependencyService.Get<IGeolocationService>();

            _searchString = "";
            FoundLocations = new ObservableCollection<Location>();
        }
        public ICommand SelectCommand => new Command<Location>(selected =>
        {
            selected.SetIndex(_list.Locations.Count + 1);
            _list.Locations.Add(selected);
            NotifyOfPropertyChange(() => _list.Locations);
            ReturnCommand.Execute(null);
        });
        public ICommand ReturnCommand => new Command(() =>
        {
            _searchString = "";
            FoundLocations.Clear();
            _navigator.PopToRootAsync();
        });

    }
}
