using UnityEngine;

namespace Sokabon.Trigger
{
    public abstract class TriggerTarget : MonoBehaviour
    {
        [SerializeField] private LayerSettings layerSettings;
        private Trigger _trigger;

        private Block _block;

        protected virtual void Awake()
        {
            _block = GetComponent<Block>();
        }

        protected virtual void Start()
        {
            CheckForTrigger();
        }

        private void OnEnable()
        {
            _block.AtNewPositionEvent += CheckForTrigger;
        }

        private void OnDisable()
        {
            _block.AtNewPositionEvent -= CheckForTrigger;
        }

        private void CheckForTrigger()
        {
            Collider2D col = Physics2D.OverlapCircle(transform.position, 0.3f, layerSettings.triggerLayerMask);
            Trigger nextTrigger = col?.GetComponent<Trigger>();
             
            if (_trigger != nextTrigger)
            {
                if (_trigger)
                {
                    OnSokabonTriggerExit(_trigger);
                }

                _trigger = nextTrigger;

                if (_trigger)
                {
                    OnSokabonTriggerEnter(_trigger);
                }
            }
        }

        protected abstract void OnSokabonTriggerEnter(Trigger trigger);
        protected abstract void OnSokabonTriggerExit(Trigger trigger);
    }
}