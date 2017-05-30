using System.Collections.Generic;
using System.Linq;
using Integreat.Shared.Services.Tracking;
using Integreat.Shared.ViewModels.Resdesign;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels
{
    public class MainTwoLevelViewModel : BaseViewModel
    {
        #region Fields

        private IList<PageViewModel> _pages;
        private PageViewModel _parentPage;
        private MainContentPageViewModel _mainContentPageViewModel;
        private List<PageViewModel> _mergedList;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the pages to be displayed.
        /// </summary>
        public IList<PageViewModel> Pages {
            get { return _pages; }
            private set { SetProperty(ref _pages, value); }
        }

        /// <summary>
        /// Gets or sets the parent page of this TwoLevelView.
        /// </summary>
        public PageViewModel ParentPage {
            get { return _parentPage; }
            private set { SetProperty(ref _parentPage, value); }
        }


        /// <summary>
        /// Gets the merged list (which is a list of the ParentPage's children + the children of those children).
        /// </summary>
        /// <value>
        /// The merged list.
        /// </value>
        public List<PageViewModel> MergedList {
            get { return _mergedList; }
            private set { SetProperty(ref _mergedList, value); }
        }

        /// <summary>
        /// Gets or sets the view model for the mainContentPage used to open new pages
        /// </summary>
        public MainContentPageViewModel MainContentPageViewModel {
            get { return _mainContentPageViewModel; }
            private set { SetProperty(ref _mainContentPageViewModel, value); }
        }

        #endregion

        public MainTwoLevelViewModel(IAnalyticsService analytics, PageViewModel parentPage, IList<PageViewModel> pages)
            : base(analytics)
        {
            Title = parentPage.Title;
            ParentPage = parentPage;
            Pages = pages;

            // merge the children and the children of those into one list (to display two levels at once)
            var mergedList = new List<PageViewModel>();
            foreach (var parentPageChild in ParentPage.Children)
            {
                parentPageChild.AccentLineHeight = 2.0;
                parentPageChild.ItemOpacity = 1.0;
                parentPageChild.ItemMargin = new Thickness(0);

                // the margin works differently on iOS and results in the labels not properly rendering
                if (Device.RuntimePlatform != Device.iOS)
                    parentPageChild.GridMargin = new Thickness(20, mergedList.Count == 0 ? 20 : 40, 20, 0); // give some extra space to the item above, but not if it's the first item in the list
                mergedList.Add(parentPageChild);
                if (parentPageChild.Children.Count == 0) continue; // continue at this point to avoid setting the last item's (which would be parentPageChild) accent line height to 0

                // add all children
                foreach (var childChild in parentPageChild.Children)
                {
                    // set the accent line for those to 1
                    childChild.AccentLineHeight = 1.0;
                    childChild.ItemMargin = new Thickness(20, 0, 0, 0);
                    // the margin works differently on iOS and results in the labels not properly rendering
                    if (Device.OS != TargetPlatform.iOS)
                        childChild.GridMargin = new Thickness(20, 0);
                    childChild.ItemOpacity = 0.8;
                    mergedList.Add(childChild);
                }
                // however set the accent line height for the last child to 0
                mergedList.Last().AccentLineHeight = 0.0;
            }
            MergedList = mergedList;
        }
    }
}
