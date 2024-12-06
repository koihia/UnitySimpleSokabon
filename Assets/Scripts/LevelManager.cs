using Sokabon;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Animator transition;
    [SerializeField] private LevelData levelData;
    public List<Level> Levels { get; } = new();
    private int _currentLevelIndex;
    
    private void Awake()
    {
        var section = 0;
        var levelInSection = 0;
        foreach (var level in levelData.levels)
        {
            if (level.sceneName == "SEPARATOR")
            {
                section++;
                levelInSection = 0;
                continue;
            }

            level.levelNumber = $"{section + 1}-{levelInSection + 1}";
            Levels.Add(level);
            
            if (level.sceneName == SceneManager.GetActiveScene().name)
            {
                _currentLevelIndex = Levels.Count - 1;
            }

            levelInSection++;
        }
    }
    
    public void LoadLevel(Level level)
    {
        LoadScene(level.sceneName);
    }

    private void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneHelper(sceneName));
    }

    private IEnumerator LoadSceneHelper(string sceneName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
    }
    
    public void RestartLevel()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToNextScene()
    {
        LoadScene(Levels[(_currentLevelIndex + 1) % Levels.Count].sceneName);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            GoToNextScene();
        }
    }
}