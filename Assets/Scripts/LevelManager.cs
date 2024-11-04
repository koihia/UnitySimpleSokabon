using Sokabon;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public string nextLevel;
    public static int CurrentLevelIndex => SceneManager.GetActiveScene().buildIndex;

    private SoundManager soundManager;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    public void RestartLevel()
    {
        StartCoroutine(RestartLevelCoroutine());
    }

    private IEnumerator RestartLevelCoroutine()
    {
        soundManager?.PlayRestartSound();
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToNextScene()
    {
        //Get the current level build Index
        int current = SceneManager.GetActiveScene().buildIndex;
        
        //increase it by one
        int next = current + 1;
        int total = SceneManager.sceneCountInBuildSettings;
        
        //If we are at the end of our list, just go back to the first level in the list.
        if (next >= total)
        {
            next = 0;
        }

        //go to build index
        SceneManager.LoadScene(next);
    }
    
    public void GoToLevel(int level)
    {
        SceneManager.LoadScene("Level" + level);
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
