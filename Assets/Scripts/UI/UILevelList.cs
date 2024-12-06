using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Sokabon.UI
{
    public class UILevelList : MonoBehaviour
    {
        public LevelManager levelManager;
        public GameObject levelListPrefab;
        public UILevelButton levelButtonPrefab;

        private void Awake()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void Start()
        {
            GameObject levelList = null;
            var prevSection = "-1";
            foreach (var level in levelManager.Levels)
            {
                var section = level.LevelNumber.Split('-')[0];
                if (section != prevSection)
                {
                    prevSection = section;
                    levelList = Instantiate(levelListPrefab, transform);
                }

                Debug.Assert(levelList != null, nameof(levelList) + " != null");
                var levelButton = Instantiate(levelButtonPrefab, levelList.transform);
                levelButton.SetLevel(levelManager, level);
            }
        }
    }
}