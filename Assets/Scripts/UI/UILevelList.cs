using UnityEngine;

namespace Sokabon.UI
{
    public class UILevelList : MonoBehaviour
    {
        public LevelManager levelManager;
        public GameObject levelButtonPrefab;

        private void Awake()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void Start()
        {
            foreach (var level in levelManager.Levels)
            {
                var levelButton = Instantiate(levelButtonPrefab, transform);
                levelButton.GetComponent<UILevelButton>().SetLevel(levelManager, level);
            }
        }
    }
}