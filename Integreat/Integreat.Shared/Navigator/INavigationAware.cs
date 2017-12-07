
namespace Integreat.Shared.Factories
{
    public interface INavigationAware
    {
        void NavigatedTo();

        void NavigatedFrom();

        void OnAppearing();
    }
}
