using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinTSP.Common.Abstractions;
using XamarinTSP.UI.Abstractions;

namespace XamarinTSP.UI.Models
{
    public class LocationList : PropertyChangedBase
    {
        public Location SelectedLocation { get; set; }
        public ObservableCollection<Location> Locations { get; set; }

        public LocationList()
        {
            Locations = new ObservableCollection<Location>();
            Locations.CollectionChanged += (s, e) => NotifyOfPropertyChange();
        }
        public ICommand DeleteCommand => new Command<Location>(location =>
        {
            Locations.Remove(location);
            location.Dispose();
        });
        public void Reorder(int[] positions)
        {
            var tmp = new Location[Locations.Count];
            for (int i = 0; i < positions.Length; i++)
            {
                tmp[i] = Locations.ElementAt(positions[i]);
                tmp[i].SetIndex(i+1);
            }
            Locations = new ObservableCollection<Location>(tmp);
        }
        public void SetMockData(IGeolocationService geolocation)
        {
            Locations.Clear();
            void addLocation(string location)
            {
                Locations.Add(new Location(Locations.Count + 1, geolocation.GetAddressList(location).FirstOrDefault()));
            }
            addLocation("Warszawa");
            addLocation("Wrocław");
            addLocation("Bydgoszcz");
            addLocation("Białystok");
            addLocation("Rzeszów");
            addLocation("Gdańsk");
            addLocation("Poznań");
            addLocation("Kraków");
            addLocation("Szczecin");
            addLocation("Lublin");
            addLocation("Katowice");
            addLocation("Łódź");
            addLocation("Koszalin");
            addLocation("Częstochowa");
            addLocation("Radom");
            addLocation("Płock");
            addLocation("Zamość");
            addLocation("Chojnice");
            addLocation("Świnoujście");
            addLocation("Słupsk");
            addLocation("Opole");
            addLocation("Zielona Góra");
            addLocation("Olsztyn");
        }
    }
}
