using System;
using UnityEngine;

namespace Sokabon.CommandSystem
{
    public class ChangeGravity : Command
    {
        private readonly BlockManager _blockManager;
        private readonly Vector2Int _currentGravityDirection;
        private readonly Vector2Int _newGravityDirection;

        public ChangeGravity(BlockManager blockManager, Vector2Int newGravityDirection)
        {
            _blockManager = blockManager;
            _currentGravityDirection = _blockManager.GravityDirection;
            _newGravityDirection = newGravityDirection;
        }
        
        public override void Execute(Action onComplete)
        {
            _blockManager.GravityDirection = _newGravityDirection;
            onComplete?.Invoke();
        }
        
        public override void Undo(Action onComplete)
        {
            _blockManager.GravityDirection = _currentGravityDirection;
            onComplete?.Invoke();
        }
    }
}