using Sokabon.Trigger;

namespace Sokabon.CommandSystem
{
    public class PickUpGelLoot : Command
    {
        private readonly Player _player;
        private readonly TriggerLootGel _triggerLootGel;

        public PickUpGelLoot(Player player, TriggerLootGel triggerLootGel)
        {
            _player = player;
            _triggerLootGel = triggerLootGel;
        }
        
        public override void Execute(System.Action onComplete)
        {
            _player._gelCount++;
            _triggerLootGel.gameObject.SetActive(false);
            onComplete?.Invoke();
        }
        
        public override void Undo(System.Action onComplete)
        {
            _player._gelCount--;
            _triggerLootGel.gameObject.SetActive(true);
            onComplete?.Invoke();
        }
    }
}