
using System.Security;

namespace Integreat.Shared.Pages
{
	public partial class LanguagesPage : BaseContentPage
    {
        [SecurityCritical]
        public LanguagesPage()
        {
            InitializeComponent();
        }
    }
}
