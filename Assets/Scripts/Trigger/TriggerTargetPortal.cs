using Sokabon.CommandSystem;
using UnityEngine;

namespace Sokabon.Trigger
{
    public class TriggerTargetPortal : TriggerTarget
    {
        [SerializeField] private LayerSettings layerSettings;
        private Block _block;

        protected override void Awake()
        {
            base.Awake();
            _block = GetComponent<Block>();
        }

        protected override void OnSokabonTriggerEnter(Trigger trigger)
        {
            var triggerPortal = trigger as TriggerPortal;
            if (triggerPortal?.destination is null)
            {
                return;
            }

            var col = Physics2D.OverlapCircle(triggerPortal.destination.transform.position, 0.3f,
                layerSettings.solidLayerMask | layerSettings.blockLayerMask | layerSettings.playerLayerMask);
            if (col is not null)
            {
                return;
            }

            _turnManager.ExecuteCommand(new Teleport(_block, triggerPortal.destination));
        }

        protected override void OnSokabonTriggerExit(Trigger trigger)
        {
            // Do nothing
        }
    }
}