using System;
using System.Diagnostics;

namespace App1.Utilities
{
    public static class AsyncErrorHandler
    {
       public static void HandleException(Exception exception)
        {
            Debug.WriteLine(exception);
        }
    }
}
