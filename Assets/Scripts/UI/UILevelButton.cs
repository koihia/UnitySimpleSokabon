using TMPro;
using UnityEngine;

namespace Sokabon.UI
{
    public class UILevelButton : MonoBehaviour
    {
        public LevelManager levelManager;
        public TextMeshProUGUI levelNumberText;
        private Level _level;
        
        public void SetLevel(LevelManager levelMgr, Level level)
        {
            levelManager = levelMgr;
            levelNumberText.text = level.levelNumber;
            _level = level;
        }
        
        public void OnClick()
        {
            levelManager.LoadLevel(_level);
        }
    }
}