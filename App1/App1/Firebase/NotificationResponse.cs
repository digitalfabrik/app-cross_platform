using System.Collections.Generic;

namespace App1.Firebase
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
