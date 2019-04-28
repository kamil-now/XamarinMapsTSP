using System.Threading.Tasks;

namespace XamarinTSP.UI.Abstractions
{
    public interface INavigator
    {
        Task PopAsync();
        Task PopToRootAsync();
        Task PushAsync<TViewModel>() where TViewModel : class, IViewModel;
        Task PushModalAsync<TViewModel>() where TViewModel : class, IViewModel;
    }
}
