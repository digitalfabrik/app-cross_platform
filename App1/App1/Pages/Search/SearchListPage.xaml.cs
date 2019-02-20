using System.Security;

namespace App1.Pages.Search
{
    /// <summary>
    /// This page is displayed for the search feature
    /// </summary>
    [SecurityCritical]
    public partial class SearchListPage
    {
        [SecurityCritical]
        public SearchListPage()
        {
            InitializeComponent();
        }
    }
}
