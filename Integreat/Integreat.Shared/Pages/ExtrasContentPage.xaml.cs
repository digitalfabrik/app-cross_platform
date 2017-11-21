using System.Security;

namespace Integreat.Shared.Pages
{
    /// <summary>
    /// ToDo merge this class with maincontent Page, they are identical
    /// </summary>
    public partial class ExtrasContentPage
	{
	    [SecurityCritical]
        public ExtrasContentPage ()
		{
			InitializeComponent ();
		}
	}
}
