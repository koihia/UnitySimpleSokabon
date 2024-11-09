using System;
using UnityEngine;

namespace Sokabon.Trigger
{
    public class TriggerDetector : MonoBehaviour
    {
        [SerializeField] private LayerSettings layerSettings;
        public Action<Trigger> OnSokabonTriggerEnterEvent;
        public Action<Trigger> OnSokabonTriggerExitEvent;

        private Trigger _trigger;

        private Block _block;

        private void Awake()
        {
            _block = GetComponent<Block>();
        }

        private void Start()
        {
            CheckForTrigger(false);
        }

        private void OnEnable()
        {
            _block.AtNewPositionEvent += CheckForTrigger;
        }

        private void OnDisable()
        {
            _block.AtNewPositionEvent -= CheckForTrigger;
        }

        private void CheckForTrigger(bool isReplay)
        {
            // TODO: detect multiple triggers
            Collider2D col = Physics2D.OverlapCircle(transform.position, 0.3f, layerSettings.triggerLayerMask);
            Trigger nextTrigger = col?.GetComponent<Trigger>();

            if (_trigger != nextTrigger)
            {
                if (_trigger && !isReplay)
                {
                    OnSokabonTriggerExitEvent?.Invoke(_trigger);
                }

                _trigger = nextTrigger;

                if (_trigger && !isReplay)
                {
                    OnSokabonTriggerEnterEvent?.Invoke(_trigger);
                }
            }
        }
    }
}