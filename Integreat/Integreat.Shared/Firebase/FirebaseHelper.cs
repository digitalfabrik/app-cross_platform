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

			return eventPage;
		}

		public string ParamsToTitle(IDictionary<string, object> parameters)
		{
			parameters.TryGetValue(_titleKey, out object title);
			return title.ToString();
		}

		public string ParamsToBody(IDictionary<string, object> parameters)
        {
			parameters.TryGetValue(_messageKey, out object body);
			return body.ToString();
        }
    }
}
