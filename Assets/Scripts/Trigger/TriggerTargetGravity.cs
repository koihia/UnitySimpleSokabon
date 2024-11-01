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

        protected override void OnTrigger(Trigger trigger)
        {
            TriggerGravity triggerGravity = trigger as TriggerGravity;
            if (triggerGravity)
            {
                _blockManager.gravityDirection = triggerGravity.GravityDirection;
            }
        }
    }
}