using UnityEngine;

namespace Sokabon.Trigger
{
    public abstract class TriggerTarget : MonoBehaviour
    {
        [SerializeField] private LayerSettings layerSettings;

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
            Trigger trigger = col?.GetComponent<Trigger>();
            OnTrigger(trigger);
        }

        protected abstract void OnTrigger(Trigger trigger);
    }
}