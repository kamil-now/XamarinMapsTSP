using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinTSP.Abstractions;
using XamarinTSP.Utilities;

namespace XamarinTSP.UI.ViewModels
{
    public class SetLocationViewModel : ViewModelBase
    {
        private IGeolocationService _geolocationService;
        private INavigator _navigator;
        private LocationList _list;
        public Location SelectedLocation { get; set; } //temp
        public ObservableCollection<Location> Locations { get; set; }

        public string EditedLocation
        {
            get => _list?.SelectedLocation?.Name ?? "";
            set
            {
                if (_list.SelectedLocation == null)
                {
                    var newLocation = new Location() { Name = value };
                    _list.SelectedLocation = newLocation;
                }
                else
                {
                    _list.SelectedLocation.Name = value;
                }
                if (!string.IsNullOrEmpty(_list.SelectedLocation.Name))
                {
                    Helper.InvokeOnMainThreadAsync(async () =>
                    {
                        var result = await _geolocationService.GetLocationList(_list.SelectedLocation.Name);
                        Locations = new ObservableCollection<Location>(result);
                        NotifyOfPropertyChange(() => Locations);
                    }, 100);
                }
                NotifyOfPropertyChange();
            }
        }
        public SetLocationViewModel(LocationList list, INavigator navigator)
        {
            _list = list;
            _navigator = navigator;
            _geolocationService = DependencyService.Get<IGeolocationService>();
            Locations = new ObservableCollection<Location>();
        }
        public ICommand SelectCommand => new Command(() =>
        {
            _list.Locations.Add(_list.SelectedLocation);
            ReturnCommand.Execute(null);
        });
        public ICommand ReturnCommand => new Command(() =>
        {
            Locations.Clear();
            _navigator.PopToRootAsync();
        });

    }
}
