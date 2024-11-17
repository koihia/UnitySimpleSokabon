using UnityEngine;

namespace Sokabon.InventorySystem
{
    [CreateAssetMenu(fileName = "Inventory Item Data", menuName = "Inventory System/Inventory Item Data", order = 0)]
    public class InventoryItemData : ScriptableObject
    {
        [field: SerializeField] public string DisplayName { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public Color IconColor { get; private set; }
        [field: SerializeField] public GameObject ItemPrefab { get; private set; }
    }
}