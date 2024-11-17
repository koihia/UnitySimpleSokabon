using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sokabon.InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        public int capacity;
        public List<InventoryItemData> Items { get; private set; }

        public Action OnInventoryItemChanges;

        public void Awake()
        {
            Items = Enumerable.Repeat<InventoryItemData>(null, capacity).ToList();

            OnInventoryItemChanges += () =>
            {
                var message = Items.Aggregate("Items: ",
                    (current, item) => current + ((item?.DisplayName ?? "Empty") + ", "));
                Debug.Log(message);
            };
        }
        
        public void Start()
        {
            OnInventoryItemChanges?.Invoke();
        }

        public int AddItem(InventoryItemData itemData)
        {
            var index = Items.FindIndex(item => item is null);
            if (index == -1)
            {
                return -1;
            }

            Items[index] = itemData;
            OnInventoryItemChanges?.Invoke();
            return index;
        }

        public bool AddItemAt(int index, InventoryItemData itemData)
        {
            if (index < 0 || index >= Items.Count || Items[index] is not null)
            {
                return false;
            }

            Items[index] = itemData;
            OnInventoryItemChanges?.Invoke();
            return true;
        }

        public bool RemoveItemAt(int index)
        {
            if (index < 0 || index >= Items.Count || Items[index] is null)
            {
                return false;
            }

            Items[index] = null;
            OnInventoryItemChanges?.Invoke();
            return true;
        }

        public bool IsItemIndexValid(int index)
        {
            return index >= 0 && index < Items.Count && Items[index] is not null;
        }
    }
}