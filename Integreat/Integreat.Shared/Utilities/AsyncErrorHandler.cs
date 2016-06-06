using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

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
