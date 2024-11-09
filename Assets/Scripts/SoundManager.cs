using UnityEngine;
using Sokabon;

public class SoundManager : MonoBehaviour
{
    public AudioClip victorySound;
    public AudioClip blockLandingSound;
    public AudioClip triggerGravitySound;
    public AudioClip restartSound;
    public AudioClip onGoalSound;
    private AudioSource audioSource;

    public GameManager gameManager;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        gameManager.OnWin += PlayVictorySound;

        var blocks = FindObjectsOfType<Block>();
        foreach (var block in blocks)
        {
            block.BlockLanding += PlayBlockLandingSound;
        }
    }

    private void OnDisable()
    {
        gameManager.OnWin -= PlayVictorySound;

        var blocks = FindObjectsOfType<Block>();
        foreach (var block in blocks)
        {
            block.BlockLanding -= PlayBlockLandingSound;
        }
    }

    private void PlayVictorySound()
    {
        audioSource.PlayOneShot(victorySound);
    }

    private void PlayBlockLandingSound()
    {
        audioSource.PlayOneShot(blockLandingSound);
    }

    public void PlayTriggerGravitySound()
    {
        audioSource.PlayOneShot(triggerGravitySound);
    }

    public void PlayRestartSound()
    {
        audioSource.PlayOneShot(restartSound);
    }

    public void PlayOnGoalSound()
    {
        audioSource.PlayOneShot(onGoalSound);
    }
}
