using Xamarin.Forms;

namespace Integreat.Shared.ViewModels.General
{
    /// <summary> Class ImagePageView model is the viewmodel for the Image page. </summary>
    public class ImagePageViewModel : BaseViewModel
    {
        private string _source;
        private double _xAxis, _yAxis;
        private Command _onPinchCommand;

        public ImagePageViewModel(string source) : base()
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

        /// <summary> Gets or sets the x axis. </summary>
        /// <value> The x axis. </value>
        public double XAxis
        {
            get => _xAxis;
            set => SetProperty(ref _xAxis, value);
        }

        /// <summary> Gets or sets the y axis. </summary>
        /// <value> The y axis. </value>
        public double YAxis
        {
            get => _yAxis;
            set => SetProperty(ref _yAxis, value);
        }

        /// <summary> Gets the on pinch command. </summary>
        /// <value> The on pinch command. </value>
        public Command OnPinchCommand => _onPinchCommand ?? (_onPinchCommand = new Command(OnPinch));

        /// <summary> Called when [pinch]. </summary>
        public void OnPinch()
        {
            //ToDo
        }
    }
}