using Android.Content;
using Android.Gms.Maps.Model;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using XamarinTSP.Droid;
using XamarinTSP.UI.CustomControls;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace XamarinTSP.Droid
{
    public class CustomMapRenderer : MapRenderer
    {
        public CustomMapRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var map = (CustomMap)e.NewElement;

                Control.GetMapAsync(this);
            }
        }
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CustomMap.RouteCoordinatesProperty))
            {
                var polylineOptions = new PolylineOptions();
                polylineOptions.InvokeColor(0x66000000);

                var coordinates = ((CustomMap)Element).RouteCoordinates;

                if (coordinates != null)
                {
                    foreach (var position in coordinates)
                        polylineOptions.Add(new LatLng(position.Latitude, position.Longitude));

                    NativeMap.AddPolyline(polylineOptions);
                }

            }
        }
    }
}