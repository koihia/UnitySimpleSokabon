namespace Sokabon.Trigger
{
    public class TriggerTargetGel : TriggerTarget
    {
        private Block _block;
        
        protected override void Awake()
        {
            base.Awake();
            _block = GetComponent<Block>();
        }
        
        protected override void OnTrigger(Trigger trigger)
        {
            _block.isAffectedByGravity = trigger is not TriggerGel;
        }
    }
}