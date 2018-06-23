using Integreat.Shared.Models.Extras.Sprungbrett;

namespace Integreat.Shared.ViewModels.Extras
{
    public class ListItemViewModel<T> : TabbableAndSelectableItemBase
    {
        public T ListItemModel { get; set; }
    }
}
