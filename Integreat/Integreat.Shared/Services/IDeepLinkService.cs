using System.Threading.Tasks;
using Integreat.Shared.Utilities;

namespace Integreat.Shared.Services
{
    /// <summary>
    /// Interface for the DeepLinkService
    /// </summary>
    public interface IDeepLinkService
    {
        Task Navigate(IShortnameParser parser);
    }
}
