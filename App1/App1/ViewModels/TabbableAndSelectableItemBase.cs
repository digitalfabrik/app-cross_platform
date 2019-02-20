using System.Windows.Input;

namespace App1.ViewModels
{
    public abstract class TabbableAndSelectableItemBase : BaseViewModel, ITabbableItem, ISelectableItemBase
    {
        public bool IsSelected { get; set; }
        public ICommand OnSelectCommand { get; set; }
        public ICommand OnTapCommand { get; set; }
    }

    public abstract class SelectableItemBase : BaseViewModel, ISelectableItemBase
    {
        public bool IsSelected { get; set; }
        public ICommand OnSelectCommand { get; set; }
    }

    public interface ISelectableItemBase
    {
        /// <summary> identify if an object is selected </summary>
        bool IsSelected { get; set; }
        /// <summary> detect selection </summary>
        ICommand OnSelectCommand { get; set; }
    }

    public abstract class TabbableITemBase : BaseViewModel, ITabbableItem
    {
        public ICommand OnTapCommand { get; set; }
    }

    public interface ITabbableItem
    {
        /// <summary>Detect Tab on item </summary>
        ICommand OnTapCommand { get; set; }
    }
}