using System;
using Sokabon.Trigger;
using UnityEngine;

namespace Sokabon.CommandSystem
{
    public class Teleport : Command
    {
        private readonly Block _block;
        private readonly Vector3 _originalPosition;
        private readonly TriggerPortal _destination;

        public Teleport(Block block, TriggerPortal destination)
        {
            _block = block;
            _originalPosition = block.transform.position;
            _destination = destination;
        }

        public override void Execute(Action onComplete)
        {
            _block.Teleport(_destination.transform.position, false, false, onComplete);
        }

        public override void Undo(Action onComplete)
        {
            _block.Teleport(_originalPosition, true, true, onComplete);
        }
    }
}