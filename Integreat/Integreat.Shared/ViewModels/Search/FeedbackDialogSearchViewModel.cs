using Integreat.Localization;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Data.Sender;
using Integreat.Shared.Models;
using Integreat.Shared.Utilities;
using Rg.Plugins.Popup.Services;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    public class FeedbackDialogSearchViewModel : BaseViewModel
    {
        private readonly DataSenderProvider _dataSenderProvider;
        protected readonly DataLoaderProvider DataLoaderProvider;
        protected readonly FeedbackFactory FeedbackFactory;

        private readonly Location _location;
        private readonly Language _language;

        private readonly string _searchString;
        private string _comment;

        public FeedbackDialogSearchViewModel(DataLoaderProvider dataLoaderProvider, DataSenderProvider dataSenderProvider, FeedbackFactory feedbackFactory,
                                             string searchString)
        {
            DataLoaderProvider = dataLoaderProvider;
            _dataSenderProvider = dataSenderProvider;
            FeedbackFactory = feedbackFactory;

            _searchString = searchString;
            ClosePopupCommand = new Command(ClosePopup);
            SendFeedbackCommand = new Command(SendFeedback);

            var locationId = Preferences.Location();
            var languageId = Preferences.Language(locationId);
            _location = DataLoaderProvider.LocationsDataLoader.Load(false).Result.FirstOrDefault(x => x.Id == locationId);
            _language = DataLoaderProvider.LanguagesDataLoader.Load(false, _location).Result.FirstOrDefault(x => x.PrimaryKey == languageId);
        }

        public ICommand ClosePopupCommand { get; }
        public ICommand SendFeedbackCommand { get; }

        public string HeadlineText => AppResources.Feedback;
        public string EditorText => AppResources.WhatIsMissing;
        public string ButtonText => AppResources.Send;

        public string Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value);
        }

        public async void SendFeedback()
        {

            var feedback = FeedbackFactory.GetFeedback(FeedbackType.Search, FeedbackKind.Up, Comment, null, _searchString);

            var errorMessage = string.Empty;
            await _dataSenderProvider.FeedbackDataSender.Send(_language, _location, feedback, FeedbackType.Search, err => errorMessage = err);


            await PopupNavigation.Instance.PopAllAsync();
            DependencyService.Get<IMessage>().ShortAlert((errorMessage != string.Empty) ? errorMessage : AppResources.FeedbackSent);
        }

        private static async void ClosePopup()
            => await PopupNavigation.Instance.PopAllAsync();
    }
}
