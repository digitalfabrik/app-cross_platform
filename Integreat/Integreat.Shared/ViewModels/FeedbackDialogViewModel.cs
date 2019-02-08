using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class FeedbackDialogViewModel : BaseViewModel
    {
        private readonly DataSenderProvider _dataSenderProvider;
        protected readonly DataLoaderProvider _dataLoaderProvider;

        private ICollection<string> _pickerItems;
        private string _selectedPickerItem;

        private readonly string _kindOfFeedback;
        private readonly FeedbackType _feedbackType;
        private readonly int _pageId;
        private readonly string _permalink;
        private string _comment;

        public FeedbackDialogViewModel(DataLoaderProvider dataLoaderProvider, DataSenderProvider dataSenderProvider, string kindOfFeedback, 
                                        FeedbackType feedbackType, int pageId = 0, string permalink = null)
        {
            _dataLoaderProvider = dataLoaderProvider;
            _dataSenderProvider = dataSenderProvider;

            _kindOfFeedback = kindOfFeedback;
            _feedbackType = feedbackType;
            _pageId = pageId;
            _permalink = permalink;
            ClosePopupCommand = new Command(ClosePopup);
            SendFeedbackCommand = new Command(SendFeedback);

            InitializePickerItems();
        }

        public ICommand ClosePopupCommand { get; }
        public ICommand SendFeedbackCommand { get; }

        public string Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value);
        }

        public string SelectedPickerItem
        {
            get => _selectedPickerItem;
            set => SetProperty(ref _selectedPickerItem, value);
        }

        public ICollection<string> PickerItems 
        {
            get => _pickerItems;
            set => SetProperty(ref _pickerItems, value);
        }

        private void InitializePickerItems() 
        {
            _pickerItems = new Collection<string>();
            PickerItems.Add("Inhalte von Augsburg");
            PickerItems.Add("Technische Funktionen");

            SelectedPickerItem = "Inhalte von Augsburg";
        }

        public async void SendFeedback()
        {
            var locationId = Preferences.Location();
            var languageId = Preferences.Language(locationId);
            Location location = (await _dataLoaderProvider.LocationsDataLoader.Load(false)).FirstOrDefault(x => x.Id == locationId);
            Language language = (await _dataLoaderProvider.LanguagesDataLoader.Load(false, location)).FirstOrDefault(x => x.PrimaryKey == languageId);

            System.Diagnostics.Debug.WriteLine("Kind of feedback: " + _kindOfFeedback);
            FeedbackPage feedback = new FeedbackPage();
            feedback.Id = _pageId;
            feedback.Permalink = _permalink;
            feedback.Comment = Comment;
            feedback.Rating = _kindOfFeedback;

            await _dataSenderProvider.FeedbackDataSender.Send(language, location, feedback, _feedbackType);
            await PopupNavigation.Instance.PopAllAsync();
        }

        private async void ClosePopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}
