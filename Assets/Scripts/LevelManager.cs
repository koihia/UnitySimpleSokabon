using Sokabon;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Animator transition;
    public string nextLevel;
    public static int CurrentLevelIndex => SceneManager.GetActiveScene().buildIndex;

    private SoundManager soundManager;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    public void RestartLevel()
    {
        GoToLevel(SceneManager.GetActiveScene().buildIndex);
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
        GoToLevel(next);
    }
    
    public void GoToLevel(int level)
    {
        LoadScene(level);
    }
    
    private void LoadScene(int sceneBuildIndex)
    {
        StartCoroutine(LoadSceneHelper(sceneBuildIndex));
    }

    private IEnumerator LoadSceneHelper(int sceneBuildIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneBuildIndex);
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
