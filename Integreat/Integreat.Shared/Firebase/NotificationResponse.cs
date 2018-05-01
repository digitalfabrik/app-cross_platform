using System;
using System.Collections.Generic;

namespace Integreat.Shared.Firebase
{
    public class NotificationResponse
    {
        public string Identifier { get; }

        public IDictionary<string, object> Data { get; }

        public NotificationResponse(IDictionary<string, object> data, string identifier = "")
        {
            Identifier = identifier;
            Data = data;
        }
    }
}
