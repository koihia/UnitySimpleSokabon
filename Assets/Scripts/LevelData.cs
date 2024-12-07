using System.Collections.Generic;
using UnityEngine;

namespace Sokabon
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Sokabon/Level Data")]
    public class LevelData : ScriptableObject
    {
        public List<Level> levels;
    }
    
    [System.Serializable]
    public class Level
    {
        public string sceneName;
        public string levelName;
        public int optimalMoves;

        [HideInInspector] public string levelNumber;
    }
}