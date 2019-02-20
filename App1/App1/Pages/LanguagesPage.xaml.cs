using System.Security;

namespace App1.Pages
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