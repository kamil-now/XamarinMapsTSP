using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinTSP.Abstractions;

namespace XamarinTSP.UI.ViewModels
{
    public class LocationListViewModel : PropertyChangedBase
    {
        public ObservableCollection<string> Locations { get; set; } = new ObservableCollection<string>() { "Wrocław", "Kraków", "Warszawa", "Katowice" };
        public string SelectedLocation { get; set; }
        public string NewLocation { get; set; }
        public ICommand SelectCommand => new Command(() =>
        {

        });
        public ICommand AddLocationCommand => new Command(() =>
        {
            Locations.Add(NewLocation);
        });
        public ICommand DeleteCommand => new Command<string>(location =>
        {
            Locations.Remove(location);
        });
    }
}
