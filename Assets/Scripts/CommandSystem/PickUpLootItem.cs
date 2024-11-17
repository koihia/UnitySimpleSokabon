using Sokabon.Trigger;

namespace Sokabon.CommandSystem
{
    public class PickUpLootItem : Command
    {
        private readonly InventorySystem.Inventory _inventory;
        private readonly TriggerLootItem _triggerLootItem;
        private int _insertedItemIndex;

        public PickUpLootItem(InventorySystem.Inventory inventory, TriggerLootItem triggerLootItem)
        {
            _inventory = inventory;
            _triggerLootItem = triggerLootItem;
        }

        public override void Execute(System.Action onComplete)
        {
            _insertedItemIndex = _inventory.AddItem(_triggerLootItem.ItemData);
            _triggerLootItem.gameObject.SetActive(false);

            onComplete?.Invoke();
        }

        public override void Undo(System.Action onComplete)
        {
            _inventory.RemoveItemAt(_insertedItemIndex);
            _triggerLootItem.gameObject.SetActive(true);

            onComplete?.Invoke();
        }
    }
}