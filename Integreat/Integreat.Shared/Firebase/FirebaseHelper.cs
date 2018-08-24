using Integreat.Localization;
using System.Collections.Generic;
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
            parameters.TryGetValue(_titleKey, out var title);
            return title != null ? title.ToString() : string.Empty;
        }

        public string ParamsToBody(IDictionary<string, object> parameters)
        {
            parameters.TryGetValue(_messageKey, out var body);
            return body != null ? body.ToString() : string.Empty;
        }

        public string ParamsToTopic(IDictionary<string, object> parameters)
        {
            parameters.TryGetValue(_topicKey, out var topic);
            return topic != null ? topic.ToString() : string.Empty;
        }

        public string ParamsToCity(IDictionary<string, object> parameters)
        {
            parameters.TryGetValue(_cityKey, out var city);
            return city != null ? city.ToString() : string.Empty;
        }

        public string ParamsToLanguage(IDictionary<string, object> parameters)
        {
            parameters.TryGetValue(_languageKey, out var language);
            return language != null ? language.ToString() : string.Empty;
        }

        public void ShowNotificationAlert(IDictionary<string, object> parameters)
        {
            var title = ParamsToTitle(parameters);
            var body = ParamsToBody(parameters);

            if (!title.IsNullOrEmpty() && !body.IsNullOrEmpty())
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage.DisplayAlert(title, body, AppResources.Close);
                });
            }
        }
    }
}
