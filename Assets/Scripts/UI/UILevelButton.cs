using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sokabon.UI
{
    public class UILevelButton : MonoBehaviour
    {
        public LevelManager levelManager;
        public TextMeshProUGUI levelNumberText;
        private Level _level;

        [SerializeField] private Image starsIndicator1;
        [SerializeField] private Image starsIndicator2;
        [SerializeField] private Image starsIndicator3;
        
        [SerializeField] private Color starColor;
        [SerializeField] private Color emptyStarColor;
        
        public void SetLevel(LevelManager levelMgr, Level level)
        {
            levelManager = levelMgr;
            levelNumberText.text = level.levelNumber;
            _level = level;
            
            var starsCount = GameDataManager.GetStarsCount(level, GameDataManager.GetBestMoves(level));
            starsIndicator1.color = starsCount >= 1 ? starColor : emptyStarColor;
            starsIndicator2.color = starsCount >= 2 ? starColor : emptyStarColor;
            starsIndicator3.color = starsCount >= 3 ? starColor : emptyStarColor;
        }
        
        public void OnClick()
        {
            levelManager.LoadLevel(_level);
        }
    }
}