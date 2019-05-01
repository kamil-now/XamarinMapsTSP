using Xamarin.Forms;

namespace XamarinTSP.UI.CustomControls
{
    public class MaterialIconButton : Button
    {
        public static readonly BindableProperty IconProperty =
            BindableProperty.Create("Icon", typeof(string), typeof(MaterialIconButton), string.Empty);
        public string Title
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
    }
}
