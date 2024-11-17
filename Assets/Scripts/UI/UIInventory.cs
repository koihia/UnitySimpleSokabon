using System;
using System.Linq;
using Sokabon.InventorySystem;
using UnityEngine;

namespace Sokabon.UI
{
    public class UIInventory : MonoBehaviour
    {
        [SerializeField] private GameObject inventoryItemSlotPrefab;
        private Inventory _inventory;

        private void Awake()
        {
            _inventory = FindObjectOfType<Inventory>();
            if (_inventory is null)
            {
                Debug.LogError("Inventory not found in scene");
            }
        }

        private void OnEnable()
        {
            _inventory.OnInventoryItemChanges += UpdateInventoryUI;
        }

        private void OnDisable()
        {
            _inventory.OnInventoryItemChanges -= UpdateInventoryUI;
        }

        private void UpdateInventoryUI()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            
            foreach (var (item, i) in _inventory.Items.Select((value, i) => ( value, i )))
            {
                var inventoryItemSlot = Instantiate(inventoryItemSlotPrefab, transform);
                inventoryItemSlot.GetComponent<UIInventoryItemSlot>().Set(item, i + 1);
            }
        }
    }
}