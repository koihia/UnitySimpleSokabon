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

        protected override void OnSokabonTriggerEnter(Trigger trigger)
        {
            TriggerLootGel triggerLootGel = trigger as TriggerLootGel;
            if (triggerLootGel is null)
            {
                return;
            }

            Destroy(triggerLootGel.gameObject);
            _player._gelCount++;
        }

        protected override void OnSokabonTriggerExit(Trigger trigger)
        {
            // Do nothing
        }
    }
}