using System;
using System.Linq;
using System.Windows.Input;
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
    //ToDo: replace all strings with App.Resoure
    public class FcmSettingsPageViewModel : BaseContentViewModel
    {
        private bool _isTopicEnabled;
        private string _TopicText;
        private string _TopicsText;
        private readonly INavigator _navigator;
        private readonly Func<FCMTopicsSettingsPageViewModel> _fcmTopicsSettingsFactory;

        public FcmSettingsPageViewModel(INavigator navigator, Func<FCMTopicsSettingsPageViewModel> fcmTopicsSettingsFactory, DataLoaderProvider dataLoaderProvider) : base(dataLoaderProvider)
        {
            _navigator = navigator;
            _fcmTopicsSettingsFactory = fcmTopicsSettingsFactory;
            OpenTopicsCommand = new Command(OpenTopics);
            _isTopicEnabled = false;
            Title = "Messaging Settings";
        }

        public string TopicText 
        {
            get => _TopicText; 

            private set
            {
                SetProperty(ref _TopicText, value);
                OnPropertyChanged(nameof(TopicText));
            }
        }

        public string ExplanationText => "To subscribe to another topic, just switch the location and/or the language";

        public string TopicsText 
        {
            get => _TopicsText; 

            private set
            {
                SetProperty(ref _TopicsText, value);
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
                    //ToDo: subscribe to topic
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
            TopicText = "Get Notifications for: " + LastLoadedLocation.Name + "(" + LastLoadedLanguage.ShortName + ")";
        }

        private void RefreshTopicsText()
        {
            TopicsText = "Edit " + FirebaseCloudMessaging.Current.SubscribedTopics.Count().ToString() + " subscriptions";
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
            return LastLoadedLocation + "-" + LastLoadedLanguage.ShortName + "-news";
        }

        protected override void LoadContent(bool forced = false, Language forLanguage = null, Location forLocation = null)
        {
            RefreshTopicText();
        }
    }
}
