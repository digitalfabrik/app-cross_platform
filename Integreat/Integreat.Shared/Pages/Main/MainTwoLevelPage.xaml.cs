using System.Security;
using Integreat.Shared.ViewModels;
using Xamarin.Forms;

namespace Integreat.Shared.Pages.Main
{
    /// <summary>
    /// ToDo I guess we will remove this page, because it is not possible to display html content in this header  
    /// </summary>
    [SecurityCritical]
    public partial class MainTwoLevelPage
    {
        [SecurityCritical]
        public MainTwoLevelPage()
        {
            InitializeComponent();
        }

        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            // TODO: Use attached behavior instead of this code-behind approach to bind the ItemTapped event to the command
            var page = (sender as ListView)?.SelectedItem as PageViewModel;
            page?.OnTapCommand.Execute(page);
        }
    }
}
