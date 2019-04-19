using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
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
            get => _list?.SelectedLocation?.MainDisplayString ?? "";
            set
            {
                if (_list.SelectedLocation == null)
                {
                    var newLocation = new Location();
                    _list.SelectedLocation = newLocation;
                }
                if (!string.IsNullOrEmpty(value))
                {
                    Helper.InvokeOnMainThreadAsync(async () =>
                    {
                        var result = await _geolocationService.GetLocationListAsync(value);
                        Locations = new ObservableCollection<Location>(result);
                        NotifyOfPropertyChange(() => Locations);
                        Locations.ForEach(x => x.DataChanged.Invoke(null, null));
                    }, 100);
                }
            }
        }
        public SetLocationViewModel(LocationList list, INavigator navigator)
        {
            _list = list;
            _navigator = navigator;
            _geolocationService = DependencyService.Get<IGeolocationService>();
            Locations = new ObservableCollection<Location>();
        }
        public ICommand SelectCommand => new Command<Location>(selected =>
        {
            _list.Locations.Add(selected);
            ReturnCommand.Execute(null);
        });
        public ICommand ReturnCommand => new Command(() =>
        {
            Locations.Clear();
            _navigator.PopToRootAsync();
        });

    }
}
