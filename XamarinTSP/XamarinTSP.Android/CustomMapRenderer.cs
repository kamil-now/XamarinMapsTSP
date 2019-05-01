using Android.Content;
using Android.Content.Res;
using Android.Gms.Maps.Model;
using Android.Graphics;
using System;
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

        private bool _rendering;
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CustomMap.RouteCoordinatesProperty) || e.PropertyName == nameof(CustomMap.LocationsProperty))
            {
                if (_rendering)
                    return;
                _rendering = true;
                NativeMap.Clear();

                _map.UpdatePins();
                _map.FocusPins();

                if (e.PropertyName == nameof(CustomMap.RouteCoordinatesProperty))
                    DrawRoute();
                _rendering = false;

            }
        }
        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            
            Bitmap bitmap = GetBitmapMarker(Context, pin.Label ?? "---");
            marker.SetIcon(BitmapDescriptorFactory.FromBitmap(bitmap));
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
        private Bitmap GetBitmapMarker(Context mContext, string mText)
        {
            try
            {
                Resources resources = mContext.Resources;
                float scale = resources.DisplayMetrics.Density;
                int w = 20, h = 20;

                Bitmap.Config bitmapConfig = Bitmap.Config.Argb8888; 
                Bitmap bitmap = Bitmap.CreateBitmap(w, h, bitmapConfig);
                
                bitmap = bitmap.Copy(bitmapConfig, true);

                Canvas canvas = new Canvas(bitmap);
                Paint paint = new Paint(PaintFlags.AntiAlias)
                {
                    Color = Android.Graphics.Color.Black,
                    TextSize = ((int)(10 * scale))
                };
                paint.SetShadowLayer(1f, 0f, 1f, Android.Graphics.Color.DarkGray);
                
                Rect bounds = new Rect();
                paint.GetTextBounds(mText, 0, mText.Length, bounds);
                int x = (bitmap.Width - bounds.Width()) / 2;
                int y = (bitmap.Height + bounds.Height()) / 2;

                canvas.DrawText(mText, x * scale, y * scale, paint);

                return bitmap;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}