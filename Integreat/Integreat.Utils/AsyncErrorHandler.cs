using System;
using System.Diagnostics;

namespace Integreat.Shared.Utilities
{
    public static class AsyncErrorHandler
    {
       public static void HandleException(Exception exception)
        {
            Debug.WriteLine(exception);
        }
    }
}
