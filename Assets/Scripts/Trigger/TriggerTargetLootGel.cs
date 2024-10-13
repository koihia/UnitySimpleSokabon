namespace Sokabon.Trigger
{
    public class TriggerTargetLootGel : TriggerTarget
    {
        private Player _player;
        
        protected override void Awake()
        {
            base.Awake();
            _player = GetComponent<Player>();
        }
        
        protected override void OnTrigger(Trigger trigger)
        {
            TriggerLootGel triggerLootGel = trigger as TriggerLootGel;
            if (triggerLootGel)
            {
                Destroy(triggerLootGel.gameObject);
                _player._gelCount++;
            }
        }
    }
}