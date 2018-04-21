using Integreat.Shared.ViewFactory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

// based on https://github.com/jamesmontemagno/Hanselman.Forms/

namespace Integreat.Shared.ViewModels
{
    /// <inheritdoc cref="IViewModel" />
    /// <summary>
    /// BaseViewmodel implementation
    /// </summary>
    public class BaseViewModel : IViewModel, IDisposable
    {
        private string _title = string.Empty;
        private string _icon;
        private bool _isBusy;
        private bool _canLoadMore = true;

        private Command _onAppearingCommand;
        private Command _metaDataChangedCommand;
        private Command _refreshCommand;


        /// <summary>
        /// Gets or sets the "Title" property
        /// </summary>
        /// <value>The _title.</value>
        public const string TitlePropertyName = "Title";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        /// <summary>
        /// Gets the description for a e.g a search result.
        /// </summary>
        public const string DescriptionPropertyName = "Description";
        public string Description => "";

        /// <summary>
        /// Gets or sets if the view is busy.
        /// </summary>
        public const string IsBusyPropertyName = "IsBusy";
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }


        /// <summary>
        /// Gets or sets the "Icon" of the viewmodel
        /// </summary>
        public const string IconPropertyName = "Icon";
        public string Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        /// <summary>
        /// Gets the size of the font.
        /// </summary>
        /// <value>
        /// The size of the font.
        /// </value>
        public double FontSize => Device.GetNamedSize(NamedSize.Large, typeof(Label));

        /// <summary>
        /// Gets or sets a value indicating whether this instance can load more.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can load more; otherwise, <c>false</c>.
        /// </value>
        public const string CanLoadMorePropertyName = "CanLoadMore";
        public bool CanLoadMore
        {
            get => _canLoadMore;
            set => SetProperty(ref _canLoadMore, value);
        }

        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="backingStore">The backing store.</param>
        /// <param name="value">The value.</param>
        /// <param name="onChanged">The on changed.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        protected bool SetProperty<T>(ref T backingStore, T value, Action onChanged = null, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value)) return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = "") 
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Navigateds to.
        /// </summary>
        public virtual void NavigatedTo()
        {
        }

        /// <summary>
        /// Navigateds from.
        /// </summary>
        public virtual void NavigatedFrom()
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // ReSharper disable once VirtualMemberNeverOverridden.Global
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            //Cleanup here
        }

        ~BaseViewModel()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets the on appearing command.
        /// </summary>
        public Command OnAppearingCommand => _onAppearingCommand ?? (_onAppearingCommand = new Command(OnAppearing));

        /// <summary>
        /// Called when [appearing].
        /// </summary>
        public virtual void OnAppearing()
        {
        }

        /// <summary> Gets the refresh command.</summary>
        public Command RefreshCommand => _refreshCommand ?? (_refreshCommand = new Command<object>((force) =>
        {
            var asBool = force as bool?;
            OnRefresh(asBool != false); // for null and true, give true. For false give false
        }));

        /// <summary> Gets the meta data changed command.</summary>
        /// <value>  The meta data changed command.</value>
        public Command MetaDataChangedCommand 
            => _metaDataChangedCommand ?? (_metaDataChangedCommand = new Command(OnMetadataChanged));

        /// <summary>
        /// Gets or sets the navigation. Set by a BasicContentPage when it's BindingContextChanged.
        /// </summary>
        /// <value> The navigation. </value>
        public INavigation Navigation { get; set; }

        /// <summary> Refreshes the content of the current page. </summary>
        /// <param name="force">if set to <c>true</c> [force] a refresh from the server.</param>
        public virtual void OnRefresh(bool force = false)
        {
        }

        /// <summary>
        /// Refreshes the content of the current page and forces to reload the selected location/language.
        /// </summary>
        protected virtual void OnMetadataChanged()
        {
        }
    }
}