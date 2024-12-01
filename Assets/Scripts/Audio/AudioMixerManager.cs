using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Sokabon.Audio
{
    public class AudioMixerManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;

        public static readonly Dictionary<AudioType, string> AudioTypeToMixerGroup = new()
        {
            { AudioType.Master, "MasterVolume" },
            { AudioType.Music, "MusicVolume" },
            { AudioType.SFX, "SFXVolume" }
        };

        private static readonly Dictionary<AudioType, float> AudioTypeToDefaultVolume = new()
        {
            { AudioType.Master, 1f },
            { AudioType.Music, 0.5f },
            { AudioType.SFX, 0.5f }
        };

        public enum AudioType
        {
            Master,
            Music,
            SFX
        }

        private void Start()
        {
            foreach (var audioType in AudioTypeToMixerGroup.Keys)
            {
                SetVolume(audioType, GetVolume(audioType));
            }
        }

        public void SetVolume(AudioType audioType, float volume)
        {
            if (volume < 0.0001)
            {
                volume = 0.0001f;
                Debug.LogWarning("Volume set to 0 is not allowed. Setting it to the minimum value instead.");
            }

            audioMixer.SetFloat(AudioTypeToMixerGroup[audioType], Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat(AudioTypeToMixerGroup[audioType], volume);
        }

        public float GetVolume(AudioType audioType)
        {
            return PlayerPrefs.GetFloat(AudioTypeToMixerGroup[audioType], AudioTypeToDefaultVolume[audioType]);
        }
    }
}