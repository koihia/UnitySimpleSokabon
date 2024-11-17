using Sokabon.CommandSystem;
using Sokabon.InventorySystem;

namespace Sokabon.Trigger
{
    public class TriggerTargetLootItem : TriggerTarget
    {
        private Inventory _inventory;

        protected override void Awake()
        {
            base.Awake();
            _inventory = GetComponent<Inventory>();
        }

        protected override void OnSokabonTriggerEnter(Trigger trigger)
        {
            var triggerLootItem = trigger as TriggerLootItem;
            if (triggerLootItem is null)
            {
                return;
            }

            _turnManager.ExecuteCommand(new PickUpLootItem(_inventory, triggerLootItem));
        }

        protected override void OnSokabonTriggerExit(Trigger trigger)
        {
            // Do nothing
        }
    }
}