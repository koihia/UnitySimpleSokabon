using Sokabon.InventorySystem;
using UnityEngine;

namespace Sokabon.Trigger
{
    public class TriggerLootItem : Trigger
    {
        [field: SerializeField]
        public InventoryItemData ItemData { get; private set; }
    }
}