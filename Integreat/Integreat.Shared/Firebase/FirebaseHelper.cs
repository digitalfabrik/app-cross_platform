using System;
using System.Collections.Generic;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Models;
using Xamarin.Forms;

namespace Integreat.Shared.Firebase
{
    public class FirebaseHelper
    {

		private string _titleKey;
		private string _messageKey;

		public FirebaseHelper()
		{
			Init();
        }

		private void Init()
		{
			if (Device.RuntimePlatform.Equals(Device.Android))
            {
                _titleKey = "title";
                _messageKey = "body";
            }
            else if (Device.RuntimePlatform.Equals(Device.iOS))
            {
                _titleKey = "aps.alert.title";
                _messageKey = "aps.alert.body";
            }
		}

		public EventPage ParamsToEventPage(IDictionary<string, object> parameters)
		{
			EventPage eventPage = new EventPage();
			eventPage.Title = ParamsToTitle(parameters);
			eventPage.Description = ParamsToBody(parameters);
            
			Event e = new Event();
			e.JsonAllDay = "true";
			e.JsonStartTime = DateTime.Now.ToString();
			e.JsonEndTime = DateTime.Now.AddDays(14).ToString();
           
			EventLocation location = new EventLocation();
			location.Address = String.Empty;

			eventPage.Location = location;         
			eventPage.Event = e;

  			return eventPage;
		}

		public string ParamsToTitle(IDictionary<string, object> parameters)
		{
			parameters.TryGetValue(_titleKey, out object title);
			return title != null ? title.ToString() : String.Empty;
		}

		public string ParamsToBody(IDictionary<string, object> parameters)
        {
			parameters.TryGetValue(_messageKey, out object body);
			return body != null ? body.ToString() : String.Empty;
        }
    }
}
