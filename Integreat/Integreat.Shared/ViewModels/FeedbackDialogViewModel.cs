using Integreat.Localization;
using Integreat.Shared.Data.Loader;
using Integreat.Shared.Data.Sender;
using Integreat.Shared.Models;
using Integreat.Shared.Utilities;
using Rg.Plugins.Popup.Services;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    public class FeedbackDialogViewModel : BaseViewModel
    {
        private readonly DataSenderProvider _dataSenderProvider;
        protected readonly DataLoaderProvider DataLoaderProvider;
        protected readonly FeedbackFactory FeedbackFactory;

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
            DataLoaderProvider = dataLoaderProvider;
            _dataSenderProvider = dataSenderProvider;
            FeedbackFactory = feedbackFactory;

            _kindOfFeedback = kindOfFeedback;
            _feedbackType = feedbackType;
            _pageId = pageId;
            _extraString = extraString;
            ClosePopupCommand = new Command(ClosePopup);
            SendFeedbackCommand = new Command(SendFeedback);

            var locationId = Preferences.Location();
            var languageId = Preferences.Language(locationId);
            _location = DataLoaderProvider.LocationsDataLoader.Load(false).Result.FirstOrDefault(x => x.Id == locationId);
            _language = DataLoaderProvider.LanguagesDataLoader.Load(false, _location).Result.FirstOrDefault(x => x.PrimaryKey == languageId);

            InitializePickerItems();
        }

        public ICommand ClosePopupCommand { get; }
        public ICommand SendFeedbackCommand { get; }

        public string HeadlineText => AppResources.Feedback;
        public string PickerText => AppResources.WhatSTheFeedbackFor;
        public string EditorText => AppResources.WhatWasHelpful;
        public string ButtonText => AppResources.Send;

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

            _pickerItems = new List<FeedbackOptionItem>();
            _pickerItems.Add(
                new FeedbackOptionItem
                {
                    Id = _pickerItems.Count,
                    Name = AppResources.ContentFrom + _location.NameWithoutStreetPrefix,
                    Type = FeedbackType.Categories
                });
            var feedbackOptionItem = PickerItems.First();
            if (_feedbackType == FeedbackType.Page)
            {
                _pickerItems.Add(
                    new FeedbackOptionItem
                    {
                        Id = _pickerItems.Count,
                        Name = AppResources.ContentOfThisPage,
                        Type = FeedbackType.Page
                    });
                feedbackOptionItem = _pickerItems.First(item => item.Type == FeedbackType.Page);
            }

            if (_feedbackType == FeedbackType.Extras)
            {
                var extras = DataLoaderProvider.ExtrasDataLoader.Load(false, _language, _location).Result;
                foreach (var extra in extras)
                {
                    _pickerItems.Add(
                        new FeedbackOptionItem
                        {
                            Id = _pickerItems.Count,
                            Name = AppResources.Extra + extra.Alias,
                            Alias = extra.Alias,
                            Type = FeedbackType.Extra
                        });

                }
                feedbackOptionItem = _pickerItems.First(item => item.Type == FeedbackType.Extra);
            }
            _pickerItems.Add(
                new FeedbackOptionItem
                {
                    Id = _pickerItems.Count,
                    Name = AppResources.TechnicalFunctions,
                    Type = FeedbackType.Categories
                });
            SelectedPickerItem = feedbackOptionItem;
        }

        public async void SendFeedback()
        {
            _feedbackType = SelectedPickerItem.Type;

            if (_feedbackType == FeedbackType.Extra)
            {
                _extraString = SelectedPickerItem.Alias;
            }

            var feedback = FeedbackFactory.GetFeedback(_feedbackType, _kindOfFeedback, Comment, _pageId, _extraString);

            var errorMessage = string.Empty;
            await _dataSenderProvider.
                FeedbackDataSender.Send(_language, _location, feedback, _feedbackType, err => errorMessage = err);


            await PopupNavigation.Instance.PopAllAsync();
            DependencyService.Get<IMessage>().
                ShortAlert((errorMessage != string.Empty) ? errorMessage : AppResources.FeedbackSent);
        }

        private static async void ClosePopup()
            => await PopupNavigation.Instance.PopAllAsync();
    }
}
