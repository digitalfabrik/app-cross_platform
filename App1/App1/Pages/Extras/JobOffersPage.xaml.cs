using System.Security;

namespace App1.Pages.Extras
{
    [SecurityCritical]
	public partial class JobOffersPage
    {
        [SecurityCritical]
        public JobOffersPage()
		{
			InitializeComponent();
		}
	}
}
