using System;
using UnityEngine;

namespace Sokabon.CommandSystem
{
    public class ChangeGravity : Command
    {
        private readonly BlockManager _blockManager;
        private readonly RectTransform _pointer;
        private readonly RectTransform _cross;
        private readonly Vector2Int _currentGravityDirection;
        private readonly Vector2Int _newGravityDirection;

        public ChangeGravity(BlockManager blockManager, RectTransform pointer, RectTransform cross, Vector2Int newGravityDirection)
        {
            _blockManager = blockManager;
            _pointer = pointer;
            _cross = cross;
            _currentGravityDirection = _blockManager.gravityDirection;
            _newGravityDirection = newGravityDirection;
        }
        
        public override void Execute(Action onComplete)
        {
            _blockManager.gravityDirection = _newGravityDirection;
            ChangePointer(_currentGravityDirection, _newGravityDirection);
            onComplete?.Invoke();
        }
        
        public override void Undo(Action onComplete)
        {
            _blockManager.gravityDirection = _currentGravityDirection;
            ChangePointer(_newGravityDirection, _currentGravityDirection);
            onComplete?.Invoke();
        }

        private void ChangePointer(Vector2Int oldGravityDirection, Vector2Int newGravityDirection)
        {
            if (oldGravityDirection == Vector2Int.zero && newGravityDirection != Vector2Int.zero)
            {
                _pointer.gameObject.SetActive(true);
                _cross.gameObject.SetActive(false);
            }
            else if (oldGravityDirection != Vector2Int.zero && newGravityDirection == Vector2Int.zero)
            {
                _pointer.gameObject.SetActive(false);
                _cross.gameObject.SetActive(true);
            }

            if (newGravityDirection != Vector2Int.zero)
            {
                float angle = Vector2.SignedAngle(Vector2.right, newGravityDirection);
                _pointer.localRotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }
}