public class ChemicalHolder : ItemHolder
{
    protected override bool ShouldBeInteractable()
        => base.ShouldBeInteractable()
        && InventoryManager.instance.currentItem.item is Item_Chemical
        && (requiredItem == null || InventoryManager.instance.currentItem.item.id == requiredItem.id);
}
