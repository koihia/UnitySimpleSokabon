using UnityEngine;

namespace Sokabon.Trigger
{
    public class TriggerTargetGravity : TriggerTarget
    {
        private BlockManager _blockManager;

        protected override void Awake()
        {
            base.Awake();
            if (_blockManager == null)
            {
                Debug.LogWarning(
                    "TriggerTargetGravity object needs BlockManager set, or BlockManager not found in scene. Searching for one.",
                    gameObject);
                _blockManager = FindObjectOfType<BlockManager>();
            }
        }

        protected override void OnSokabonTriggerEnter(Trigger trigger)
        {
            var triggerGravity = trigger as TriggerGravity;
            if (triggerGravity is null)
            {
                return;
            }
            
            _blockManager.gravityDirection = triggerGravity.GravityDirection;
        }

        protected override void OnSokabonTriggerExit(Trigger trigger)
        {
            // Do nothing
        }
    }
}