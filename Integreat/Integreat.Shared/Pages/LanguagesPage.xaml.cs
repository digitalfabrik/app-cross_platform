using System.Security;

namespace Integreat.Shared.Pages
{
    /// <summary>
    /// Backend class for LanguagesPage
    /// </summary>
    /// <inheritdoc />
    public partial class LanguagesPage
    {
        [SecurityCritical]
        public LanguagesPage()
        {
            InitializeComponent();
        }
    }
}