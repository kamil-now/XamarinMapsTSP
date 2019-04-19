using Android.Content;
using Android.Gms.Maps.Model;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using XamarinTSP.Droid;
using XamarinTSP.UI.ViewModels;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace XamarinTSP.Droid
{
    public class CustomMapRenderer : MapRenderer
    {
        List<Position> routeCoordinates;

        public CustomMapRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                // Unsubscribe
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                routeCoordinates = formsMap?.List?.Locations?.Select(x => x.Position)?.ToList();
                Control.GetMapAsync(this);
            }
        }

        protected override void OnMapReady(Android.Gms.Maps.GoogleMap map)
        {
            base.OnMapReady(map);
            if (routeCoordinates == null)
                return;

            var polylineOptions = new PolylineOptions();
            polylineOptions.InvokeColor(0x66FF0000);
            foreach (var position in routeCoordinates)
            {
                polylineOptions.Add(new LatLng(position.Latitude, position.Longitude));
            }

            NativeMap.AddPolyline(polylineOptions);
        }
    }
}