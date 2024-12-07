using UnityEngine;

namespace Sokabon.UI
{
    public class UIOverlayCloseOnKeyboardInput : MonoBehaviour
    {
        [SerializeField] private UIOverlay uiOverlay;

        private void Awake()
        {
            uiOverlay ??= GetComponent<UIOverlay>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                uiOverlay.Close();
            }
        }
    }
}