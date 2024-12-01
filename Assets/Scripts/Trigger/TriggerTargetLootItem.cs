using Sokabon.CommandSystem;
using Sokabon.InventorySystem;
using UnityEngine;

namespace Sokabon.Trigger
{
    public class TriggerTargetLootItem : TriggerTarget
    {
        [SerializeField] private AudioClip[] pickUpLootItemSounds;

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
            _SfxManager?.PlayRandom(pickUpLootItemSounds);
        }

        protected override void OnSokabonTriggerExit(Trigger trigger)
        {
            // Do nothing
        }
    }
}