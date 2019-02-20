using System.Security;

namespace App1.Pages
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
