using System.Windows.Input;
using Xamarin.Forms;
using XamarinTSP.Abstractions;
using XamarinTSP.Utilities;

namespace XamarinTSP.UI.ViewModels
{
    public class LocationListViewModel : PropertyChangedBase
    {
        private INavigator _navigator;

        public LocationList List { get; set; }

        public LocationListViewModel(LocationList list, INavigator navigator)
        {
            _navigator = navigator;
            List = list;
        }

        public ICommand SelectCommand => new Command(() =>
        {

        });
        public ICommand AddLocationCommand => new Command(async () =>
        {
            await _navigator.PushAsync<SelectLocationViewModel>();
        });
        public ICommand DeleteCommand => new Command<Location>(location =>
        {
            List.Locations.Remove(location);
        });
    }
}
