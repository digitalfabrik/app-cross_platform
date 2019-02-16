using System.Windows.Input;
using Integreat.Model.Utilities;

namespace Integreat.Model
{
    public class MenuItem : ObservableObject
    {
        public string Id { get; set; }
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        private string _subtitle;
        public string Subtitle
        {
            get => _subtitle;
            set => SetProperty(ref _subtitle, value);
        }

        public string Icon { get; set; }
        public string Parameter { get; set; }

        public AppPage Page { get; set; }
        public ICommand Command { get; set; }
    }
}
