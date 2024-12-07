using System;
using UnityEngine;

namespace Sokabon.UI
{
    public class UILosePanelInput : MonoBehaviour
    {
        [SerializeField] private TurnManager turnManager;
        
        private void Awake()
        {
            turnManager ??= FindObjectOfType<TurnManager>();
        }
        
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                turnManager.Undo();
            }
        }
    }
}