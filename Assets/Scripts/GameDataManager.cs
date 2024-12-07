using UnityEngine;

namespace Sokabon
{
    public class GameDataManager : MonoBehaviour
    {
        [SerializeField] private TurnManager turnManager;
        [SerializeField] private LevelManager levelManager;
        
        private void Awake()
        {
            turnManager ??= FindObjectOfType<TurnManager>();
            levelManager ??= FindObjectOfType<LevelManager>();
        }

        public void UpdateBestMoves()
        {
            UpdateBestMoves(levelManager.CurrentLevel, turnManager.TurnCount);
        }
        
        public static void UpdateBestMoves(Level level, int moves)
        {
            var bestMoves = GetBestMoves(level);
            if (bestMoves == -1 || bestMoves > moves)
            {
                PlayerPrefs.SetInt(level.sceneName, moves);
            }
        }
        
        public static int GetBestMoves(Level level)
        {
            return PlayerPrefs.GetInt(level.sceneName, -1);
        }
        
        public static int GetStarsCount(Level level, int moves)
        {
            if (moves == -1)
            {
                return 0;
            }

            if (moves <= level.optimalMoves * 1.1f)
            {
                return 3;
            }
            if (moves <= level.optimalMoves * 1.5f)
            {
                return 2;
            }
            return 1;
        }
    }
}