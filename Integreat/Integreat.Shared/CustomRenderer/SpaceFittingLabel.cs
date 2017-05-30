using System.Security;
using Xamarin.Forms;

namespace Integreat.Shared.CustomRenderer
{
    /// <summary>
    /// Custom renderer label, which adjusts it's size to the given width, depending on the max. amount of lines.
    /// </summary>
    [SecurityCritical]
    public class SpaceFittingLabel : Label {

        /// <summary>
        /// Gets or sets the maximal text size.
        /// </summary>
        public int MaximalTextSize { get; set; }

        /// <summary>
        /// Gets or sets the minimal text size.
        /// </summary>
        public int MinimalTextSize { get; set; }

        /// <summary>
        /// Gets or sets the amount of lines the label may use.
        /// </summary>
        public int MaximalLineCount { get; set; }
    }
}
