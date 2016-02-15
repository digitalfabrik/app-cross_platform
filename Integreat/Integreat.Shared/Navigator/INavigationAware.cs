
namespace Integreat.Shared.ViewFactory
{
    public interface INavigationAware
    {
        void NavigatedTo();

        void NavigatedFrom();

        void OnAppearing();
    }
}
