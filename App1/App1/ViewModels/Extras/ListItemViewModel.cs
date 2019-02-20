namespace App1.ViewModels.Extras
{
    public class ListItemViewModel<T> : TabbableAndSelectableItemBase
    {
        public T ListItemModel { get; set; }
    }
}
