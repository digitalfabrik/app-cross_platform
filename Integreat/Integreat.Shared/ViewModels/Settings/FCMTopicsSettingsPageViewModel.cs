using System;
using System.Collections.Generic;
using System.Windows.Input;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Firebase;
using Integreat.Shared.Utilities;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    public class FCMTopicsSettingsPageViewModel : BaseViewModel
    {
        private DataLoaderProvider _dataLoaderProvider;

        public FCMTopicsSettingsPageViewModel(DataLoaderProvider dataLoaderProvider)
        {
            _dataLoaderProvider = dataLoaderProvider;
            Title = "Topics";
            DeleteTopicCommand = new Command(DeleteTopic);
        }

        public ICommand DeleteTopicCommand;

        public string HeadingText => "Your current subscriptions";

        public string DeleteText => "Delete";

        public IList<TopicListItem> Topics => GetCurrentTopics();

        private void DeleteTopic(object sender)
        {
            FirebaseCloudMessaging.Current.Unsubscribe(((TopicListItem)sender).TopicString);
            OnPropertyChanged(nameof(Topics));
        }

        private IList<TopicListItem> GetCurrentTopics()
        {
            IList<TopicListItem> topicList = new List<TopicListItem>();

            foreach (string topicString in FirebaseCloudMessaging.Current.SubscribedTopics)
            {
                topicList.Add(new TopicListItem(topicString, _dataLoaderProvider));
            }

            return topicList;
        }
    }
}
