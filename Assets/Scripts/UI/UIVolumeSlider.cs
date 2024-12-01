using Sokabon.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Sokabon.UI
{
    public class UIVolumeSlider : MonoBehaviour
    {
        [SerializeField] private AudioMixerManager audioMixerManager;
        [SerializeField] private AudioMixerManager.AudioType audioType;

        private void Awake()
        {
            if (audioMixerManager is null)
            {
                audioMixerManager = FindObjectOfType<AudioMixerManager>();
                Debug.LogWarning(
                    "AudioMixerManager reference is not set in UIVolumeSlider. Trying to find one in the scene.");
            }
        }

        private void Start()
        {
            GetComponent<Slider>().value = audioMixerManager.GetVolume(audioType);
        }

        public void OnValueChanged(float value)
        {
            audioMixerManager.SetVolume(audioType, value);
        }
    }
}