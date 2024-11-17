using System;
using UnityEngine;

namespace Sokabon.CommandSystem
{
    public class ResetInertiaDirection : Command
    {
        private readonly Block _block;
        private Vector2Int _oldPreviousDirection;

        public ResetInertiaDirection(Block block)
        {
            _block = block;
        }

        public override void Execute(Action onComplete)
        {
            _oldPreviousDirection = _block.previousMoveDirection;
            _block.previousMoveDirection = Vector2Int.zero;
            onComplete?.Invoke();
        }

        public override void Undo(Action onComplete)
        {
            _block.previousMoveDirection = _oldPreviousDirection;
            onComplete?.Invoke();
        }
    }
}