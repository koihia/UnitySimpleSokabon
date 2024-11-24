using System;
using System.Collections;
using System.Collections.Generic;
using Sokabon.CommandSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sokabon
{
    public class Block : MonoBehaviour
    {
        public Action<bool> AtNewPositionEvent;
        public Action BlockLanding;
        private BlockManager _blockManager;
        [SerializeField] private MovementSettings movementSettings;

        public Transform sprite;
        private Rigidbody2D _rigidbody2D;
        public bool IsAnimating => _animating; 
        private bool _animating;
        
        public bool isAffectedByGravity = true;
        public bool isAffectedByInertia = false;
        public Vector2Int previousMoveDirection;
        
        [SerializeField] private LayerSettings layerSettings;

        private void Awake()
        {
            if (_blockManager == null)
            {
                Debug.LogWarning(
                    "Block object needs BlockManager set, or BlockManager not found in scene. Searching for one.",
                    gameObject);
                _blockManager = FindObjectOfType<BlockManager>();
            }
            
            sprite = transform.GetChild(0);
            if (sprite is null)
            {
                Debug.LogError("SpriteRenderer not found in children of Block object.", gameObject);
            }
            
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public Vector2 GetPosInDir(Vector2Int direction)
        {
            return _rigidbody2D.position + direction;
        }

        public void MoveInDirection(Vector2Int direction, bool instant, bool isReplay, Action onComplete)
        {
            var destination = GetPosInDir(direction);

            if (instant)
            {
                _rigidbody2D.position = destination;
                AtNewPositionEvent?.Invoke(isReplay);
                onComplete?.Invoke();
            }
            else
            {
                StartCoroutine(AnimateMove(direction, destination, isReplay, onComplete));
            }
        }

        public IEnumerator AnimateMove(Vector2Int direction, Vector2 destination, bool isReplay, Action onComplete)
        {
            _animating = true;
            
            Vector3 start = _rigidbody2D.position;
            sprite.position = start;
            _rigidbody2D.position = destination;
            
            float t = 0;
            while (t < 1)
            {
                t = t + Time.deltaTime/movementSettings.timeToMove;
                sprite.position = Vector3.Lerp(start, destination, movementSettings.movementCurve.Evaluate(t));
                yield return null;
            }

            sprite.position = destination;
            AtNewPositionEvent?.Invoke(isReplay);
            onComplete?.Invoke();
            _animating = false;

            if (GetComponent<Player>() == null && _blockManager.GravityDirection == direction &&
                !IsDirectionFree(direction))
            {
                BlockLanding?.Invoke();
            }
        }

        public void Teleport(Vector3 destination, bool instant, bool isReplay, Action onComplete)
        {
            // TODO: Animate teleport
            _rigidbody2D.position = destination;
            AtNewPositionEvent?.Invoke(isReplay);
            onComplete?.Invoke();
        }

        public bool IsDirectionFree(Vector2Int direction)
        {
            var position = GetPosInDir(direction);
            //the ^ combines the two bitmaps with an OR (the pipe symbol) bitwise operation. 0011 and 0110 becomes 0111. 
            //It means we are checking for blocks AND solid stuff, and have the flexibility to differentiate.
            Collider2D col2D = Physics2D.OverlapCircle(position, 0.3f, layerSettings.solidLayerMask | layerSettings.blockLayerMask);
            return col2D == null;
        }

        public Block BlockInDirection(Vector2Int direction)
        {
            var position = GetPosInDir(direction);
            Collider2D col2D = Physics2D.OverlapCircle(position, 0.3f, layerSettings.blockLayerMask);
            if (col2D != null)
            {
                return col2D.GetComponent<Block>();
            }
            else
            {
                return null;
            }
        }
    }
}