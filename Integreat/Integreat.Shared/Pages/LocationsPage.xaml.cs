using System.Security;

namespace Integreat.Shared.Pages
{
    /// <summary>
    /// Backend class for LocationsPage
    /// </summary>
    /// <inheritdoc />
    public partial class LocationsPage
    {
        [SecurityCritical]
        public LocationsPage()
        {
            InitializeComponent();
        }
    }
}
