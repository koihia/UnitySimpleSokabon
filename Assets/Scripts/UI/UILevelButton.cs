using TMPro;
using UnityEngine;

namespace Sokabon.UI
{
    public class UILevelButton : MonoBehaviour
    {
        public LevelManager levelManager;
        public TextMeshProUGUI levelNumberText;
        private LevelMetaData _levelMetaData;
        
        public void SetLevel(LevelManager levelMgr, LevelMetaData levelMetaData)
        {
            levelManager = levelMgr;
            levelNumberText.text = levelMetaData.LevelNumber;
            _levelMetaData = levelMetaData;
        }
        
        public void OnClick()
        {
            levelManager.LoadLevel(_levelMetaData);
        }
    }
}