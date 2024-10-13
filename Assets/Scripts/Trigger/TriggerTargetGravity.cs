using UnityEngine;

namespace Sokabon.Trigger
{
    public class TriggerTargetGravity : TriggerTarget
    {
        protected override void OnTrigger(Trigger trigger)
        {
            TriggerGravity triggerGravity = trigger as TriggerGravity;
            if (triggerGravity)
            {
                Block.GravityDirection = triggerGravity.GravityDirection;
            }
        }
    }
}