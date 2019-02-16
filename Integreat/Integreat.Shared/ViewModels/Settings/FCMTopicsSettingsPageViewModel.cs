using Integreat.Localization;
using Integreat.Shared.Firebase;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Integreat.Data.Loader;
using Integreat.Data.Utilities;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    public class FcmTopicsSettingsPageViewModel : BaseViewModel
    {
        private readonly DataLoaderProvider _dataLoaderProvider;

        public FcmTopicsSettingsPageViewModel(DataLoaderProvider dataLoaderProvider)
        {
            _dataLoaderProvider = dataLoaderProvider;
            Title = AppResources.FirebaseName;
            DeleteTopicCommand = new Command(DeleteTopic);
        }

        public ICommand DeleteTopicCommand { get; }

        public string HeadingText => AppResources.PushSub;

        public string DeleteText => AppResources.Delete;

        public ObservableCollection<TopicListItem> Topics => GetCurrentTopics();

        private void DeleteTopic(object sender)
        {
            FirebaseCloudMessaging.Current.Unsubscribe(((TopicListItem)sender).TopicString);
            OnPropertyChanged(nameof(Topics));
        }

        private ObservableCollection<TopicListItem> GetCurrentTopics()
        {
            var topicList = new ObservableCollection<TopicListItem>();

            foreach (var topicString in FirebaseCloudMessaging.Current.SubscribedTopics)
            {
                topicList.Add(new TopicListItem(topicString, _dataLoaderProvider));
            }

            return topicList;
        }
    }
}