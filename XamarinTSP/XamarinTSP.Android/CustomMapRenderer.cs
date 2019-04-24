using Android.Content;
using Android.Gms.Maps.Model;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using XamarinTSP.Droid;
using XamarinTSP.UI.CustomControls;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace XamarinTSP.Droid
{
    public class CustomMapRenderer : MapRenderer
    {
        private CustomMap _map;
        public CustomMapRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                _map = (CustomMap)e.NewElement;
                _map.FocusPins();
                Control.GetMapAsync(this);
            }
        }
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CustomMap.RouteCoordinatesProperty) || e.PropertyName == nameof(CustomMap.LocationsProperty))
            {
                NativeMap.Clear();

                _map.UpdatePins();
                _map.FocusPins();

                if (e.PropertyName == nameof(CustomMap.RouteCoordinatesProperty))
                    DrawRoute();

            }
        }
        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));

            marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.ic_room_black_24dp));
            return marker;
        }
        

        private void DrawRoute()
        {
            var polylineOptions = new PolylineOptions();
            polylineOptions.InvokeColor(0x66000000);

            var coordinates = _map.RouteCoordinates;

            if (coordinates != null)
            {
                foreach (var position in coordinates)
                    polylineOptions.Add(new LatLng(position.Latitude, position.Longitude));

                NativeMap.AddPolyline(polylineOptions);
            }
        }
    }
}