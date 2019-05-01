using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinTSP.Droid;
using XamarinTSP.UI.CustomControls;

[assembly: ExportRenderer(typeof(MaterialIconButton), typeof(MaterialIconButtonRenderer))]
namespace XamarinTSP.Droid
{
    public class MaterialIconButtonRenderer : ButtonRenderer
    {
        private readonly Context _context;
        private MaterialIconButton _button;
        public MaterialIconButtonRenderer(Context context) : base(context)
        {
            _context = context;
        }

        
        //protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        //{
        //    base.OnElementChanged(e);
        //    if (e.NewElement != null)
        //    {
        //        var button = e.NewElement as MaterialIconButton;
        //        var inflater = _context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;
        //        var rootLayout = inflater.Inflate(Resource.Layout.MaterialIconButton, null, false);
        //        SetBackground(rootLayout);
        //        SetNativeControl(rootLayout);
        //    }
        //}

        //private void SetBackground(Android.Views.View rootLayout)
        //{
        //    // Get the background color from Forms element
        //    var backgroundColor = Element.BackgroundColor.ToAndroid();

        //    // Create statelist to handle ripple effect
        //    var enabledBackground = new GradientDrawable(GradientDrawable.Orientation.LeftRight, new int[] { backgroundColor, backgroundColor });
        //    var stateList = new StateListDrawable();
        //    var rippleItem = new RippleDrawable(ColorStateList.ValueOf(Android.Graphics.Color.White),
        //                                        enabledBackground,
        //                                        null);
        //    stateList.AddState(new[] { Android.Resource.Attribute.StateEnabled }, rippleItem);

        //    // Assign background
        //    rootLayout.Background = stateList;
        //}
    }
}