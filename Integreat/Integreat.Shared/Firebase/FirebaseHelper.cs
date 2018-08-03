using System;
using System.Collections.Generic;
using System.Linq;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Models;
using Integreat.Shared.Utilities;
using Xamarin.Forms;

namespace Integreat.Shared.Firebase
{
    public class FirebaseHelper
    {
		private string _titleKey;
		private string _messageKey;
		private string _topicKey;
		private string _cityKey;
		private string _languageKey;

		public FirebaseHelper()
		{
			Init();
        }

		private void Init()
		{
            _titleKey = "title";
            _messageKey = "body";
    		_topicKey = "topic";
    		_cityKey = "city";
    		_languageKey = "lanCode";
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

		public string ParamsToTopic(IDictionary<string, object> parameters)
        {
            parameters.TryGetValue(_topicKey, out object topic);
            return topic != null ? topic.ToString() : String.Empty;
        }
        
		public string ParamsToCity(IDictionary<string, object> parameters)
        {
            parameters.TryGetValue(_cityKey, out object city);
            return city != null ? city.ToString() : String.Empty;
        }

		public string ParamsToLanguage(IDictionary<string, object> parameters)
        {
			parameters.TryGetValue(_languageKey, out object language);
			return language != null ? language.ToString() : String.Empty;
        }

		public void ShowNotificationAlert(IDictionary<string, object> parameters)
        {
            var title = ParamsToTitle(parameters);
            var body = ParamsToBody(parameters);

            if (!title.IsNullOrEmpty() && !body.IsNullOrEmpty())
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IntegreatApp.Current.MainPage.DisplayAlert(title, body, "Cancel");
                });
            }
        }
    }
}
