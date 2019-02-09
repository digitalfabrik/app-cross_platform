namespace Integreat.Shared.ViewModels
{
    public class ListItemViewModel<T> : TabbableAndSelectableItemBase
    {
        public T ListItemModel { get; set; }
    }
}
