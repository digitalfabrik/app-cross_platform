using System.Linq;
using System.Windows.Input;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Data.Sender;
using Integreat.Shared.Models;
using Integreat.Shared.Models.Feedback;
using Integreat.Shared.Utilities;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    public class FeedbackDialogSearchViewModel : BaseViewModel
    {
        private readonly DataSenderProvider _dataSenderProvider;
        protected readonly DataLoaderProvider _dataLoaderProvider;
        protected readonly FeedbackFactory _feedbackFactory;

        private readonly Location _location;
        private readonly Language _language;

        private readonly string _searchString;
        private string _comment;

        public FeedbackDialogSearchViewModel(DataLoaderProvider dataLoaderProvider, DataSenderProvider dataSenderProvider, FeedbackFactory feedbackFactory, 
                                             string searchString)
        {
            _dataLoaderProvider = dataLoaderProvider;
            _dataSenderProvider = dataSenderProvider;
            _feedbackFactory = feedbackFactory;

            _searchString = searchString;
            ClosePopupCommand = new Command(ClosePopup);
            SendFeedbackCommand = new Command(SendFeedback);

            var locationId = Preferences.Location();
            var languageId = Preferences.Language(locationId);
            _location = _dataLoaderProvider.LocationsDataLoader.Load(false).Result.FirstOrDefault(x => x.Id == locationId);
            _language = _dataLoaderProvider.LanguagesDataLoader.Load(false, _location).Result.FirstOrDefault(x => x.PrimaryKey == languageId);
        }

        public ICommand ClosePopupCommand { get; }
        public ICommand SendFeedbackCommand { get; }

        public string Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value);
        }

        public async void SendFeedback()
        {

            IFeedback feedback = _feedbackFactory.GetFeedback(FeedbackType.Search, FeedbackKind.Up, Comment, null, _searchString);

            string errorMessage = string.Empty;
            await _dataSenderProvider.FeedbackDataSender.Send(_language, _location, feedback, FeedbackType.Search, err => errorMessage = err);

            await PopupNavigation.Instance.PopAllAsync();
            DependencyService.Get<IMessage>().ShortAlert((errorMessage != string.Empty)?errorMessage:"Feedback sent");
        }

        private async void ClosePopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}
