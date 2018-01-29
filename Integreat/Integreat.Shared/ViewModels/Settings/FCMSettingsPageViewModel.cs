using System.Linq;
using System.Windows.Input;
using Integreat.Shared.Firebase;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    /// <inheritdoc />
    /// <summary>
    /// Class FCMSettingsPageViewModel contains all information and functionality for the  FCMSettingsPage
    /// </summary>
    public class FCMSettingsPageViewModel:BaseViewModel
    {
        private string _topics;

        public FCMSettingsPageViewModel()
        {
            AddTopicCommand = new Command(TopicCommand);
            UnsubscribeAllCommand = new Command(UnsubscribeAll);
        }

        public string TopicText { get; set; }

        public string Topics { get => _topics; private set => SetProperty(ref _topics, value); }

        public ICommand AddTopicCommand { get; }
        public ICommand UnsubscribeAllCommand { get; }


        public void TopicCommand(object obj)
        {
            if (TopicText.IsNullOrEmpty()) return;
            FirebaseCloudMessaging.Current.Subscribe(TopicText);
            RefreshTopics();
        }

        public void UnsubscribeAll(object obj)
        {
            FirebaseCloudMessaging.Current.UnsubscribeAll();
            RefreshTopics();
        }

        private void RefreshTopics()
        {
            var topics = FirebaseCloudMessaging.Current.SubscribedTopics.Aggregate(string.Empty, (current, topic) => current + "|" + topic);
            Topics = topics;
        }
    }
}
