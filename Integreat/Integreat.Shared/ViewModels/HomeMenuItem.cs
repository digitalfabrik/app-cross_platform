using Xamarin.Forms;

namespace Integreat.Shared.Models
{
    public class HomeMenuItem : BaseModel
    {
        public HomeMenuItem()
        {
            PageId = -1;
        }
        public string Icon { get; set; }
        public int PageId { get; set; }
        public UriImageSource ImageSource
        {
            get;
            set;
        }
    }
}
