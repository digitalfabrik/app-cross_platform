using Integreat.Shared.ViewFactory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Integreat.Shared.Services.Tracking;
using Xamarin.Forms;

// based on https://github.com/jamesmontemagno/Hanselman.Forms/

namespace Integreat.Shared.ViewModels
{
    public class BaseViewModel : IViewModel, IDisposable
    {
        private readonly IAnalyticsService _analyticsService;

        public BaseViewModel(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        private string _title = string.Empty;
        public const string TitlePropertyName = "Title";

        /// <summary>
        /// Gets or sets the "Title" property
        /// </summary>
        /// <value>The _title.</value>
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _subtitle = string.Empty;

        /// <summary>
        /// Gets or sets the "Subtitle" property
        /// </summary>
        public const string SubtitlePropertyName = "Subtitle";

        public string Subtitle
        {
            get { return _subtitle; }
            set { SetProperty(ref _subtitle, value); }
        }

        private string _icon;

        /// <summary>
        /// Gets or sets the "Icon" of the viewmodel
        /// </summary>
        public const string IconPropertyName = "Icon";

        public string Icon
        {
            get { return _icon; }
            set { SetProperty(ref _icon, value); }
        }

        private UriImageSource _imageSource;

        /// <summary>
        /// Gets or sets the "ImageSource" of the viewmodel
        /// </summary>
        public const string ImageSourcePropertyName = "ImageSource";

        public UriImageSource ImageSource
        {
            get { return _imageSource; }
            set { SetProperty(ref _imageSource, value); }
        }


        private bool _isBusy;

        /// <summary>
        /// Gets or sets if the view is busy.
        /// </summary>
        public const string IsBusyPropertyName = "IsBusy";

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        private bool _canLoadMore = true;

        /// <summary>
        /// Gets or sets if we can load more.
        /// </summary>
        public const string CanLoadMorePropertyName = "CanLoadMore";

        public bool CanLoadMore
        {
            get { return _canLoadMore; }
            set { SetProperty(ref _canLoadMore, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            changed?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void NavigatedTo()
        {
        }

        public virtual void NavigatedFrom()
        {
        }

        public virtual void Dispose()
        {
        }

        private Command _onAppearingCommand;
        public Command OnAppearingCommand => _onAppearingCommand ?? (_onAppearingCommand = new Command(OnAppearing));
        public virtual void OnAppearing()
        {
            _analyticsService.TrackPage(Title);
        }

        private Command _refreshCommand;
        /// <summary>
        /// Gets the refresh command.
        /// </summary>
        /// <value>
        /// The refresh command.
        /// </value>
        public Command RefreshCommand => _refreshCommand ?? (_refreshCommand = new Command<object>((force) =>
        {
            var asBool = force as bool?;
            OnRefresh(asBool != false); // for null and true, give true. For false give false
        }));

        private Command _metaDataChangedCommand;
        /// <summary>
        /// Gets the meta data changed command.
        /// </summary>
        /// <value>
        /// The meta data changed command.
        /// </value>
        public Command MetaDataChangedCommand => _metaDataChangedCommand ?? (_metaDataChangedCommand = new Command(() => OnMetadataChanged()));

        /// <summary>
        /// Gets or sets the navigation. Set by a BasicContentPage when it's BindingContextChanged.
        /// </summary>
        /// <value>
        /// The navigation.
        /// </value>
        public INavigation Navigation { get; set; }

        /// <summary>
        /// Refreshes the content of the current page.
        /// </summary>
        /// <param name="force">if set to <c>true</c> [force] a refresh from the server.</param>
        public virtual void OnRefresh(bool force = false)
        {
        }

        /// <summary>
        /// Refreshes the content of the current page and forces to reload the selected location/language.
        /// </summary>
        protected virtual void OnMetadataChanged() {
        }

    }
}
