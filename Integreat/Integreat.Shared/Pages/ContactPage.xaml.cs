using System.Security;

namespace Integreat.Shared.Pages
{

    public partial class ContactPage : BaseContentPage
    {
        [SecuritySafeCritical]
        public ContactPage()
        {
            InitializeComponent();
        }
    }
}