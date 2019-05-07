using Android.Content;
using Android.Gms.Maps.Model;
using Android.Graphics;
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
        private CustomMap _map;
        private bool _isRendering;
        public CustomMapRenderer(Context context) : base(context) { }

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
            if (e.PropertyName == nameof(CustomMap.RouteCoordinatesProperty) 
                || e.PropertyName == nameof(CustomMap.LocationsProperty)
                || e.PropertyName == nameof(CustomMap.IsRouteVisibleProperty))
            {
                if (_isRendering)
                {
                    return;
                }
                _isRendering = true;
                NativeMap.Clear();

                _map.UpdatePins();
                _map.FocusPins();

                if (_map.IsRouteVisible)
                {
                    DrawRoute();
                }
                else
                {
                    ClearRoute();
                }
                _isRendering = false;

            }
        }
        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));

            Bitmap bitmap = GetBitmapMarker(Context, Resource.Drawable.black_pin_full, pin.Label);
            marker.SetIcon(BitmapDescriptorFactory.FromBitmap(bitmap));
            return marker;
        }


        private void DrawRoute()
        {
            var polylineOptions = new PolylineOptions();
            polylineOptions.InvokeColor(0x66000000);

            var coordinates = _map.RouteCoordinates;

            coordinates?.ForEach(position => polylineOptions.Add(new LatLng(position.Latitude, position.Longitude)));

            if (coordinates != null)
            {
                NativeMap.AddPolyline(polylineOptions);
            }
        }
        private void ClearRoute()
        {
            var polylineOptions = new PolylineOptions();
            NativeMap.AddPolyline(polylineOptions);
        }
        private Bitmap GetBitmapMarker(Context context, int icon, string text)
        {
            var resources = context.Resources;
            float scale = resources.DisplayMetrics.Density;
            var bitmap = BitmapFactory.DecodeResource(resources, icon);

            Bitmap.Config bitmapConfig = bitmap.GetConfig();

            if (bitmapConfig == null)
            {
                bitmapConfig = Bitmap.Config.Argb8888;
            }
            bitmap = bitmap.Copy(bitmapConfig, true);

            var canvas = new Canvas(bitmap);
            var paint = new Paint(PaintFlags.AntiAlias | PaintFlags.FakeBoldText)
            {
                Color = Android.Graphics.Color.White,
                TextSize = (int)(12 * scale),
            };
            paint.SetShadowLayer(1f, 0f, 1f, Android.Graphics.Color.DarkGray);
            var bounds = new Rect();
            paint.GetTextBounds(text, 0, text.Length, bounds);

            //center in full pin icon top circle
            float x = (float)((bitmap.Width - bounds.Width()) / 2.2);
            float y = (float)((bitmap.Height + bounds.Height()) / 2.5);
            canvas.DrawText(text, x, y, paint);

            return bitmap;
        }
    }
}