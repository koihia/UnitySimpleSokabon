using Sokabon.CommandSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sokabon.Trigger
{
    public class TriggerTargetGel : TriggerTarget
    {
        [SerializeField] private AudioClip[] onGelSounds;

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

            _turnManager.ExecuteCommand(new ChangeIsAffectedByGravity(_block, false));
            _SfxManager?.PlayRandom(onGelSounds);
        }
        
        protected override void OnSokabonTriggerExit(Trigger trigger)
        {
            if (trigger is not TriggerGel)
            {
                return;
            }

            _turnManager.ExecuteCommand(new ChangeIsAffectedByGravity(_block, true));
        }
    }
}