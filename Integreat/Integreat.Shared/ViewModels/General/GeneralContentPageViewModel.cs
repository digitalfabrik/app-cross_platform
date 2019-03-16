using System;
using Integreat.Shared.Models;
using Integreat.Shared.Services;

namespace Integreat.Shared.ViewModels
{
    public class GeneralContentPageViewModel : BaseWebViewViewModel
    {
        private string _content;
        private Page _page;

        public GeneralContentPageViewModel(INavigator navigator,
            Func<string, ImagePageViewModel> imagePageFactory,
            Func<string, PdfWebViewPageViewModel> pdfWebViewFactory, string content, MainContentPageViewModel mainContentPageViewModel) :
            base(navigator, imagePageFactory, pdfWebViewFactory, mainContentPageViewModel)
            {
                _content = content;
            }

        public Page Page
        {
            get { return _page; }
            set => SetProperty(ref _page, value);
        }

        public string Content
        {
            get { return _content; }
            set => SetProperty(ref _content, value);
        }
    }
}
