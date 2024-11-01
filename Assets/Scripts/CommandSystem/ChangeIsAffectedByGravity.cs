using System;
using UnityEngine;

namespace Sokabon.CommandSystem
{
    public class ChangeIsAffectedByGravity : Command
    {
        private readonly Block _block;
        private readonly bool _currentIsAffectedByGravity;
        private readonly bool _newIsAffectedByGravity;

        public ChangeIsAffectedByGravity(Block block, bool newIsAffectedByGravity)
        {
            _block = block;
            _currentIsAffectedByGravity = _block.isAffectedByGravity;
            _newIsAffectedByGravity = newIsAffectedByGravity;
        }

        public override void Execute(Action onComplete)
        {
            _block.isAffectedByGravity = _newIsAffectedByGravity;
            onComplete?.Invoke();
        }
        
        public override void Undo(Action onComplete)
        {
            _block.isAffectedByGravity = _currentIsAffectedByGravity;
            onComplete?.Invoke();
        }
    }
}