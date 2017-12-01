using System.Collections.Generic;
using Integreat.Shared.Services;
using Xamarin.Forms;
using Page = Integreat.Shared.Models.Page;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Integreat.Shared.ViewModels
{
    public class PageViewModel : BaseViewModel
    {
        #region Fields

        private readonly INavigator _navigator;
        private List<PageViewModel> _children;

        private Command _onTapCommand;
        #endregion

        public PageViewModel(INavigator navigator, Page page)
        {
            Title = page.Title;
            _navigator = navigator;
            Page = page;
            OnTapCommand = new Command(ShowPage);
        }

        #region Properties

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public Page Page { get; set; }

        public string Content => Page.Content;

        // ReSharper disable once UnusedMember.Global
        public string Thumbnail => Page.Thumbnail;

        public Command OnTapCommand
        {
            get => _onTapCommand;
            set => SetProperty(ref _onTapCommand, value);
        }

        public List<PageViewModel> Children
        {
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
        /// Gets a value indicating whether this instance has meaningful content.
        /// </summary>
        public bool HasContent => !string.IsNullOrWhiteSpace(Content);

        #endregion

        public async void ShowPage(object modal)
        {

            await _navigator.PushAsync(this);
            if ("Modal".Equals(modal?.ToString()))
            {
                await _navigator.PopModalAsync();
            }
        }
    }
}
