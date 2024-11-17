using Sokabon.InventorySystem;
using UnityEngine;

namespace Sokabon.CommandSystem
{
    public class PutItem : Command
    {
        private readonly Inventory _inventory;
        private readonly int _itemIndex;
        private readonly InventoryItemData _inventoryItemData;
        private GameObject _instantiatedItem;

        public PutItem(Inventory inventory, int itemIndex)
        {
            _inventory = inventory;
            _itemIndex = itemIndex;
            _inventoryItemData = _inventory.Items[itemIndex];
            IsPlayerInput = true;
        }
        
        public override void Execute(System.Action onComplete)
        {
            _inventory.RemoveItemAt(_itemIndex);
            _instantiatedItem = Object.Instantiate(_inventoryItemData.ItemPrefab, _inventory.transform.position, Quaternion.identity);
            
            onComplete?.Invoke();
        }
        
        public override void Undo(System.Action onComplete)
        {
            _inventory.AddItemAt(_itemIndex, _inventoryItemData);            
            Object.Destroy(_instantiatedItem);

            onComplete?.Invoke();
        }
    }
}