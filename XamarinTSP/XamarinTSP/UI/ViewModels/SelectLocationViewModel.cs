using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinTSP.Abstractions;
using XamarinTSP.Utilities;

namespace XamarinTSP.UI.ViewModels
{
    public class SelectLocationViewModel : ViewModelBase
    {
        private string _searchText;
        private IGeolocationService _geolocationService;
        private INavigator _navigator;
        private LocationList _list;

        public Location SelectedLocation { get; set; }
        public ObservableCollection<Location> Locations { get; set; }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                if (!string.IsNullOrEmpty(_searchText))
                {
                    Helper.InvokeOnMainThreadAsync(async () =>
                    {
                        var result = await _geolocationService.GetLocationList(_searchText);
                        Locations = new ObservableCollection<Location>(result);
                        NotifyOfPropertyChange(() => Locations);
                    }, 100);
                }
                NotifyOfPropertyChange();
            }
        }
        public SelectLocationViewModel(LocationList list, INavigator navigator)
        {
            _list = list;
            _navigator = navigator;
            _geolocationService = DependencyService.Get<IGeolocationService>();
            Locations = new ObservableCollection<Location>();
        }
        public ICommand SelectCommand => new Command(() =>
        {
            //TODO parameter
            //if (selected is Location location)
            _list.Locations.Add(SelectedLocation);
            ReturnCommand.Execute(null);

        });
        public ICommand ReturnCommand => new Command(() =>
        {
            _searchText = "";
            Locations.Clear();
            _navigator.PopToRootAsync();
        });

    }
}
