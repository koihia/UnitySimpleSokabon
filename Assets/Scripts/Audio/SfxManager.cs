using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sokabon.Audio
{
    public class SfxManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        public void Play(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }
        
        public void PlayRandom(AudioClip[] clips)
        {
            audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
        }
    }
}