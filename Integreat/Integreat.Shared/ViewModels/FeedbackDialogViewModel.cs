using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Data.Sender;
using Integreat.Shared.Models;
using Integreat.Shared.Models.Extras;
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
        protected readonly FeedbackFactory _feedbackFactory;

        private List<FeedbackOptionItem> _pickerItems;
        private FeedbackOptionItem _selectedPickerItem;

        private readonly Location _location;
        private readonly Language _language;

        private readonly FeedbackKind _kindOfFeedback;
        private FeedbackType _feedbackType;
        private readonly int? _pageId;
        private string _extraString;
        private string _comment;

        public FeedbackDialogViewModel(DataLoaderProvider dataLoaderProvider, DataSenderProvider dataSenderProvider, FeedbackFactory feedbackFactory, 
                                        FeedbackKind kindOfFeedback, FeedbackType feedbackType, int? pageId, string extraString)
        {
            _dataLoaderProvider = dataLoaderProvider;
            _dataSenderProvider = dataSenderProvider;
            _feedbackFactory = feedbackFactory;

            _kindOfFeedback = kindOfFeedback;
            _feedbackType = feedbackType;
            _pageId = pageId;
            _extraString = extraString;
            ClosePopupCommand = new Command(ClosePopup);
            SendFeedbackCommand = new Command(SendFeedback);

            var locationId = Preferences.Location();
            var languageId = Preferences.Language(locationId);
            _location = _dataLoaderProvider.LocationsDataLoader.Load(false).Result.FirstOrDefault(x => x.Id == locationId);
            _language = _dataLoaderProvider.LanguagesDataLoader.Load(false, _location).Result.FirstOrDefault(x => x.PrimaryKey == languageId);

            InitializePickerItems();
        }

        public ICommand ClosePopupCommand { get; }
        public ICommand SendFeedbackCommand { get; }

        //Todo: change to AppResources
        public string HeadlineText => "Feedback";
        public string PickerText => "Wofür ist das Feedback?";
        public string EditorText => "Was war hilfreich?";
        public string ButtonText => "Send";

        public string Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value);
        }

        public FeedbackOptionItem SelectedPickerItem
        {
            get => _selectedPickerItem;
            set => SetProperty(ref _selectedPickerItem, value);
        }

        public List<FeedbackOptionItem> PickerItems 
        {
            get => _pickerItems;
            set => SetProperty(ref _pickerItems, value);
        }

        private void InitializePickerItems() 
        {
            //Todo: change to AppResources
            _pickerItems = new List<FeedbackOptionItem>();
            _pickerItems.Add(new FeedbackOptionItem { Id = _pickerItems.Count, Name = "Inhalte von: " + _location.NameWithoutStreetPrefix, Type = FeedbackType.Categories });
            FeedbackOptionItem si = PickerItems.First();
            if(_feedbackType == FeedbackType.Page) 
            {
                _pickerItems.Add(new FeedbackOptionItem { Id = _pickerItems.Count, Name = "Inhalte der Seite", Type = FeedbackType.Page });
                si = _pickerItems.First(item => item.Type == FeedbackType.Page);
            }

            if (_feedbackType == FeedbackType.Extras)
            {
                var extras = _dataLoaderProvider.ExtrasDataLoader.Load(false, _language, _location).Result;
                foreach(Extra extra in extras) {
                    _pickerItems.Add(new FeedbackOptionItem { Id = _pickerItems.Count, Name = "Extra: " + extra.Alias, Alias = extra.Alias, Type = FeedbackType.Extra });

                }
                si = _pickerItems.First(item => item.Type == FeedbackType.Extra);
            }
            _pickerItems.Add(new FeedbackOptionItem { Id = _pickerItems.Count, Name = "Technische Funktionen", Type = FeedbackType.Categories});
            SelectedPickerItem = si;
        }

        public async void SendFeedback()
        {
            if(_feedbackType != SelectedPickerItem.Type) {
                _feedbackType = SelectedPickerItem.Type;
            }

            if(_feedbackType == FeedbackType.Extra) {
                _extraString = SelectedPickerItem.Alias;
            }

            IFeedback feedback = _feedbackFactory.GetFeedback(_feedbackType, _kindOfFeedback, Comment, _pageId, _extraString);

            string errorMessage = string.Empty;
            await _dataSenderProvider.FeedbackDataSender.Send(_language, _location, feedback, _feedbackType, err => errorMessage = err);

            //Todo: AppResources
            await PopupNavigation.Instance.PopAllAsync();
            DependencyService.Get<IMessage>().ShortAlert((errorMessage != string.Empty)?errorMessage:"Feedback sent");
        }

        private async void ClosePopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}
