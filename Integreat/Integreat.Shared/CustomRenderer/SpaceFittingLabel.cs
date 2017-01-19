using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Integreat.Shared.CustomRenderer
{
    /// <summary>
    /// Custom renderer label, which adjusts it's size to the given width, depending on the max. amount of lines.
    /// </summary>
    public class SpaceFittingLabel : Label {
        /// <summary>
        /// Gets or sets the width which is available for the text.
        /// </summary>
        public double AvailableWidth { get; set; }

        /// <summary>
        /// Gets or sets the amount of lines the label may use.
        /// </summary>
        public uint AvailableLines { get; set; }
    }
}
