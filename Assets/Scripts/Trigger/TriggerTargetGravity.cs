using Sokabon.CommandSystem;
using UnityEngine;

namespace Sokabon.Trigger
{
    public class TriggerTargetGravity : TriggerTarget
    {
        [SerializeField] private RectTransform pointer;
        [SerializeField] private RectTransform cross;
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
            
            _turnManager.ExecuteCommand(new ChangeGravity(_blockManager, pointer, cross, triggerGravity.GravityDirection));
            _soundManager?.PlayTriggerGravitySound();
        }

        protected override void OnSokabonTriggerExit(Trigger trigger)
        {
            // Do nothing
        }
    }
}