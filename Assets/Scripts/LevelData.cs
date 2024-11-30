using System.Collections.Generic;
using UnityEngine;

namespace Sokabon
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Sokabon/Level Data")]
    public class LevelData : ScriptableObject
    {
        public List<LevelDataEntry> levels;
    }
    
    [System.Serializable]
    public class LevelDataEntry
    {
        public string sceneName;
        public string levelName;
    }
}