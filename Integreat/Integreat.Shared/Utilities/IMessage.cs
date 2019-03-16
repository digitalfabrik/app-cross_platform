using System;
namespace Integreat.Shared.Utilities
{
    public interface IMessage
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }
}
