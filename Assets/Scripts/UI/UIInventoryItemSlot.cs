using Sokabon.InventorySystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sokabon.UI
{
    public class UIInventoryItemSlot : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI slotNumberText;
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI label;

        public void Awake()
        {
            slotNumberText.text = 0.ToString();
            itemIcon.sprite = null;
            itemIcon.color = Color.clear;
            label.text = string.Empty;
        }

        public void Set(InventoryItemData itemData, int slotNumber)
        {
            slotNumberText.text = slotNumber.ToString();
            itemIcon.sprite = itemData?.Icon;
            itemIcon.color = itemData?.IconColor ?? Color.clear;
            label.text = itemData?.DisplayName ?? "Empty";
        }
    }
}