using System.Collections.Generic;
using System.Linq;
using Integreat.Shared.Services;
using Integreat.Shared.Services.Tracking;
using Xamarin.Forms;
using Page = Integreat.Shared.Models.Page;

namespace Integreat.Shared.ViewModels {
    public class PageViewModel : BaseViewModel {
        #region Fields

        private readonly INavigator _navigator;
        private readonly IDialogProvider _dialogProvider;

        private Command _onTapCommand;
        #endregion

        #region Properties

        public Page Page { get; set; }

        public string Content => Page.Content;

        public string Description => Page.Description;
        public string Thumbnail => Page.Thumbnail;

        public Command OnTapCommand {
            get => _onTapCommand;
            set => SetProperty(ref _onTapCommand, value);
        }

        public List<PageViewModel> Children {
            get => _children;
            set => SetProperty(ref _children, value);
        }

        /// <summary>
        /// Used to style two level view's accent lines
        /// </summary>
        public double AccentLineHeight { get; set; } = 2.0;

        /// <summary>
        /// Used to style two level view's item margins
        /// </summary>
        public Thickness ItemMargin { get; set; }

        /// <summary>
        /// Used to style two level view's item margins, however not with labels but with the whole grid
        /// </summary>
        public Thickness GridMargin { get; set; }
        /// <summary>
        /// Used to style two level view's text colors (via opacity)
        /// </summary>
        public double ItemOpacity { get; set; }

        /// <summary>
        /// Gets the height in pixel if all of this items children were to be displayed in the TwoLevelView
        /// </summary>
        public double ChildrenHeight => Children.Count * 75;

        /// <summary>
        /// Gets the height for the twoLevelView, when this page + all of it's children were to be displayed
        /// </summary>
        public double TwoLevelChildrenHeight
        {
            get
            {
                var toReturn = 0.0;
                foreach (var child in Children)
                {
                    toReturn += 130;
                    toReturn = child.Children.Aggregate(toReturn, (current, secondLevelChild) => current + 75);
                }
                return toReturn;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has meaningful content.
        /// </summary>
        public bool HasContent => !string.IsNullOrWhiteSpace(Content);

        #endregion


        public PageViewModel(IAnalyticsService analytics, INavigator navigator, Models.Page page, IDialogProvider dialogProvider)
        : base(analytics) {
            Title = page.Title;
            _navigator = navigator;
            _dialogProvider = dialogProvider;
            Page = page;
            OnTapCommand = new Command(ShowPage);
        }

        public async void ShowPage(object modal) {

            await _navigator.PushAsync(this);
            if ("Modal".Equals(modal?.ToString())) {
                await _navigator.PopModalAsync();
            }
        }

        private Command _openSearchCommand;
        public Command OpenSearchCommand => _openSearchCommand ?? (_openSearchCommand = new Command(OnSearchClicked));

        private void OnSearchClicked() {
        }

        private Command _changeLanguageCommand;
        private Command _changeLocalLanguageCommand;
        private List<PageViewModel> _children;
        public Command ChangeLanguageCommand => _changeLanguageCommand ?? (_changeLanguageCommand = new Command(OnChangeLanguageClicked));

        public Command ChangeLocalLanguageCommand {
            get => _changeLocalLanguageCommand;
            set => _changeLocalLanguageCommand = value;
        }

        // command that gets executed, when the user wants to change the language for this page instance. Sends this as parameter

        private void OnChangeLanguageClicked() {
            if (Page.AvailableLanguages.IsNullOrEmpty()) {
            }

            /*
            var availableLanguages = Page.AvailableLanguages.Select(x => x.OtherPage.Language.Name).ToArray();
            //if (availableLanguages.Length == 0) return;
            var action = await _dialogProvider.DisplayActionSheet("Select a Language?", "Cancel", null,
                        availableLanguages);


            var selectedLanguage = Page.AvailableLanguages.FirstOrDefault(x => x.OtherPage.Language.Name.Equals(action));
            if (selectedLanguage != null)
            {
                Page = selectedLanguage.OtherPage;
            }
            else
            {
                Console.WriteLine("No language selected");
            }*/
        }
    }
}
