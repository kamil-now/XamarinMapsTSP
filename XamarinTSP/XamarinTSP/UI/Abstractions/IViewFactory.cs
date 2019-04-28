using Xamarin.Forms;

namespace XamarinTSP.UI.Abstractions
{
    public interface IViewFactory
    {
        void Register<TViewModel, TView>() where TViewModel : class, IViewModel where TView : Page;
        Page Resolve<TViewModel>() where TViewModel : class, IViewModel;
    }
}
