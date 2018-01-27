using System;
using System.Windows.Input;
using Integreat.Shared.Firebase;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    /// <summary>
    /// Class FCMSettingsPageViewModel contains all information and functionality for the  FCMSettingsPage
    /// </summary>
    public class FCMSettingsPageViewModel:BaseViewModel
    {
        private string _topicText;
        private string _topics;

        public FCMSettingsPageViewModel()
        {
            AddTopicCommand = new Command(TopicCommand);
            UnsubscribeAllCommand = new Command(UnsubscribeAll);
        }

        public string TopicText { get => _topicText; set => _topicText = value; }
        public string Topics { get => _topics; private set => SetProperty(ref _topics, value); }

        public ICommand AddTopicCommand { get; }
        public ICommand UnsubscribeAllCommand { get; }


        public void TopicCommand(object obj)
        {
            if (!TopicText.IsNullOrEmpty())
            {
                FirebaseCloudMessaging.Current.Subscribe(TopicText);
                RefreshTopics();
            }
        }

        public void UnsubscribeAll(object obj)
        {
            FirebaseCloudMessaging.Current.UnsubscribeAll();
            RefreshTopics();
        }

        private void RefreshTopics()
        {
            var s = String.Empty;
            foreach (string topic in FirebaseCloudMessaging.Current.SubscribedTopics)
            {
                s = s + "|" + topic;
            }
            Topics = s;
        }
    }
}
