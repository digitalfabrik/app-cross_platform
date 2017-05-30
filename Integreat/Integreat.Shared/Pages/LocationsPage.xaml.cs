using System.Security;
using Xamarin.Forms;

namespace Integreat.Shared.Pages
{
    public partial class LocationsPage : BaseContentPage
    {
        [SecurityCritical]
        public LocationsPage()
        {
            InitializeComponent();
        }
    }
}
