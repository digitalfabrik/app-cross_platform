using System;
using System.Windows.Input;
using Integreat.Shared.Firebase;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    public class FCMTopicsSettingsPageViewModel : BaseViewModel
    {
        public FCMTopicsSettingsPageViewModel()
        {
            Title = "Topics";
            DeleteTopicCommand = new Command(DeleteTopic);
        }

        public ICommand DeleteTopicCommand;

        public string HeadingText => "Your current subscriptions";

        public string DeleteText => "Delete";

        public string[] Topics => FirebaseCloudMessaging.Current.SubscribedTopics;

        private void DeleteTopic(object sender)
        {
            //Todo delete topics
            OnPropertyChanged(nameof(Topics));
        }
    }
}
