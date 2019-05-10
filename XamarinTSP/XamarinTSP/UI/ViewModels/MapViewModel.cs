using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;
using XamarinTSP.Common.Abstractions;
using XamarinTSP.UI.Abstractions;
using XamarinTSP.UI.Models;

namespace XamarinTSP.UI.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        private IGeolocationService _geolocation;
        private Position _mapPosition;

        public double ZoomDistance { get; set; }
        public bool IsRouteVisible { get; set; }
        public Route CalculatedRoute { get; set; }
        public LocationList List { get; set; }

        public Position MapPosition
        {
            get => _mapPosition; set
            {
                _mapPosition = value;
                NotifyOfPropertyChange();
            }
        }

        public MapViewModel(IGeolocationService geolocation, LocationList list)
        {
            _geolocation = geolocation;
            List = list;
            CalculatedRoute = new Route();
        }
        
        public void DisplayRoute()
        {
            IsRouteVisible = true;
            NotifyOfPropertyChange(() => CalculatedRoute.RouteCoordinates);
            NotifyOfPropertyChange(() => CalculatedRoute.Time);
            NotifyOfPropertyChange(() => CalculatedRoute.Distance);
        }
        public async Task MoveToUserRegion()
        {

            await MoveToLocation(RegionInfo.CurrentRegion.DisplayName);
        }
        public async Task MoveToLocation(string locationName)
        {
            var positions = await _geolocation.GetLocationCoordinates(locationName);
            if (positions != null)
            {
                MapPosition = new Position(positions.First().Latitude, positions.First().Longitude);
                NotifyOfPropertyChange(() => MapPosition);
            }
        }
    }
}
