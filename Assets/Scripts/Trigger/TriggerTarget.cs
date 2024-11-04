using UnityEngine;

namespace Sokabon.Trigger
{
    public abstract class TriggerTarget : MonoBehaviour
    {
        TriggerDetector _triggerDetector;
        protected TurnManager _turnManager;
        protected SoundManager _soundManager;
        
        protected virtual void Awake()
        {
            _triggerDetector = GetComponent<TriggerDetector>();
            _turnManager = FindObjectOfType<TurnManager>();
            _soundManager = FindObjectOfType<SoundManager>();
        }

        private void OnEnable()
        {
            _triggerDetector.OnSokabonTriggerEnterEvent += OnSokabonTriggerEnter;
            _triggerDetector.OnSokabonTriggerExitEvent += OnSokabonTriggerExit;
        }

        private void OnDisable()
        {
            _triggerDetector.OnSokabonTriggerEnterEvent -= OnSokabonTriggerEnter;
            _triggerDetector.OnSokabonTriggerExitEvent -= OnSokabonTriggerExit;
        }
        
        protected abstract void OnSokabonTriggerEnter(Trigger trigger);
        protected abstract void OnSokabonTriggerExit(Trigger trigger);
    }
}