using System;
using System.Linq;
using System.Windows.Input;
using Integreat.Localization;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Firebase;
using Integreat.Shared.Models;
using Integreat.Shared.Services;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Integreat.Shared.ViewModels
{
    /// <inheritdoc />
    /// <summary>
    /// Class FCMSettingsPageViewModel contains all information and functionality for the  FCMSettingsPage
    /// </summary>
    public class FcmSettingsPageViewModel : BaseContentViewModel
    {
        private bool _isTopicEnabled;
        private string _topicText;
        private string _topicsText;
        private readonly INavigator _navigator;
        private readonly Func<FcmTopicsSettingsPageViewModel> _fcmTopicsSettingsFactory;

        public FcmSettingsPageViewModel(INavigator navigator, Func<FcmTopicsSettingsPageViewModel> fcmTopicsSettingsFactory, DataLoaderProvider dataLoaderProvider) : base(dataLoaderProvider)
        {
            _navigator = navigator;
            _fcmTopicsSettingsFactory = fcmTopicsSettingsFactory;
            OpenTopicsCommand = new Command(OpenTopics);
            _isTopicEnabled = false;
            Title = AppResources.FirebaseName;
        }

        public string TopicText 
        {
            get => _topicText; 

            private set
            {
                SetProperty(ref _topicText, value);
                OnPropertyChanged(nameof(TopicText));
            }
        }

        public string ExplanationText => AppResources.FCMExplanation;

        public string TopicsText 
        {
            get => _topicsText; 

            private set
            {
                SetProperty(ref _topicsText, value);
                OnPropertyChanged(nameof(TopicsText));
            }
        }

        public bool IsTopicEnabled 
        {
            get => _isTopicEnabled; 

            set
            {
                if(IsTopicEnabled !=value)
                {
                    if (value)
                        FirebaseCloudMessaging.Current.Subscribe(BuildTopicString());
                    else
                        FirebaseCloudMessaging.Current.Unsubscribe(BuildTopicString());

                    RefreshTopicsText();
                    _isTopicEnabled = value;
                    OnPropertyChanged(nameof(IsTopicEnabled));
                }
            }
        }

        public ICommand OpenTopicsCommand { get; }

        public override void OnAppearing()
        {
            RefreshTopicText();
            RefreshTopicsText();
            RefreshSwitch();
            base.OnAppearing();
        }

        private async void OpenTopics(object obj)
        {
            await _navigator.PushAsync(_fcmTopicsSettingsFactory(), Navigation);
        }

        private void RefreshTopicText()
        {
            TopicText = AppResources.GetNotificationsFor + " " + LastLoadedLocation.Name + "(" + LastLoadedLanguage.ShortName + ")";
        }

        private void RefreshTopicsText()
        {
            TopicsText = AppResources.EditSubscriptions;
        }


        private void RefreshSwitch()
        {
            if (FirebaseCloudMessaging.Current.SubscribedTopics.IndexOf(BuildTopicString()) > -1)
                IsTopicEnabled = true;
            else
                IsTopicEnabled = false;
        }

        private string BuildTopicString()
        {
            return LastLoadedLocation.Id + "-" + LastLoadedLanguage.ShortName + "-news";
        }

        protected override void LoadContent(bool forced = false, Language forLanguage = null, Location forLocation = null)
        {
            RefreshTopicText();
        }
    }
}
