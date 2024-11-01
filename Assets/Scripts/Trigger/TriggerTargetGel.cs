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
        
        protected override void OnSokabonTriggerEnter(Trigger trigger)
        {
            if (trigger is not TriggerGel)
            {
                return;
            }
            
            _block.isAffectedByGravity = false;
        }
        
        protected override void OnSokabonTriggerExit(Trigger trigger)
        {
            if (trigger is not TriggerGel)
            {
                return;
            }

            _block.isAffectedByGravity = true;
        }
    }
}