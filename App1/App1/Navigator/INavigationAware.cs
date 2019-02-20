
namespace App1.Navigator
{
    public interface INavigationAware
    {
        void NavigatedTo();

        void NavigatedFrom();

        void OnAppearing();
    }
}
