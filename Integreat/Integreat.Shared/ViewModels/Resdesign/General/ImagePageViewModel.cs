using System;
using System.Windows.Input;
using Integreat.Shared.Services.Tracking;
using Xamarin.Forms;

namespace Integreat.Shared.ViewModels.Resdesign.General
{
    /// <summary> Class ImagePageView model is the viewmodel for the Image page. </summary>
    public class ImagePageViewModel : BaseViewModel
    {
        private string _source;
        private double _xAxis, _yAxis;
        private Command _onPinchCommand;

        public ImagePageViewModel(IAnalyticsService analyticsService, string source) : base(analyticsService)
        {
            Source = source;
        }

        /// <summary> Gets or sets the Image path. </summary>
        /// <value> The source path. </value>
        public string Source
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }

        public double XAxis
        {
            get => _xAxis;
            set => SetProperty(ref _xAxis, value);
        }

        public double YAxis
        {
            get => _yAxis;
            set => SetProperty(ref _yAxis, value);
        }

        public Command OnPinchCommand => _onPinchCommand ?? (_onPinchCommand = new Command(OnPinch));

        public void OnPinch()
        {
           //ToDo
        }
    }
}