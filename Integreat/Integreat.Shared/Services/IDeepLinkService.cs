using Integreat.Shared.Utilities;

namespace Integreat.Shared.Services
{
    /// <summary>
    /// Interface for the DeepLinkService
    /// </summary>
    public interface IDeepLinkService
    {
        void Navigate(IShortnameParser parser);
    }
}
