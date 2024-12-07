using UnityEngine;

namespace Sokabon.UI
{
    public class UISettingsPanelInput : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        
        private void Awake()
        {
            gameManager ??= FindObjectOfType<GameManager>();
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameManager.UnPause();
            }
        }
    }
}