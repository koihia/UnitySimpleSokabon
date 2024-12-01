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
        private BlockManager _blockManager;
        [SerializeField] private MovementSettings movementSettings;

        public Transform sprite;
        private Transform _movementIndicator;
        public Rigidbody2D rb;
        public bool IsAnimating => _animating; 
        private bool _animating;

        public bool isAffectedByGravity;
        public bool isAffectedByInertia = false;
        public Vector2Int previousMoveDirection;
        public Vector2Int nextMoveDirection;
        
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

            sprite = transform.Find("Sprite");
            if (sprite is null)
            {
                Debug.LogError("SpriteRenderer not found in children of Block object.", gameObject);
            }

            _movementIndicator = transform.Find("Sprite/Next Move Direction Indicator");
            if (_movementIndicator is null)
            {
                Debug.LogError("Movement Indicator not found in children of Block object.", gameObject);
            }

            rb = GetComponent<Rigidbody2D>();
        }

        public Vector2 GetPosInDir(Vector2Int direction)
        {
            return rb.position + direction;
        }

        public void MoveInDirection(Vector2Int direction, bool instant, bool isReplay, Vector2Int? previousDirection,
            Action onComplete)
        {
            var destination = GetPosInDir(direction);
            previousMoveDirection = previousDirection ?? direction;

            if (instant)
            {
                rb.position = destination;
                AtNewPositionEvent?.Invoke(isReplay);
                onComplete?.Invoke();
            }
            else
            {
                StartCoroutine(AnimateMove(destination, isReplay, onComplete));
            }
        }

        public IEnumerator AnimateMove(Vector2 destination, bool isReplay, Action onComplete)
        {
            _animating = true;
            
            Vector3 start = rb.position;
            sprite.position = start;
            rb.position = destination;
            
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
        }

        public void Teleport(Vector3 destination, bool instant, bool isReplay, Action onComplete)
        {
            // TODO: Animate teleport
            rb.position = destination;
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

        public void UpdateNextMoveDirection()
        {
            nextMoveDirection = Vector2Int.zero;

            if (isAffectedByInertia && previousMoveDirection != Vector2Int.zero)
            {
                if (IsDirectionFree(previousMoveDirection))
                {
                    nextMoveDirection = previousMoveDirection;
                }
                else
                {
                    // XXX: Is bypassing the command system here safe?
                    previousMoveDirection = Vector2Int.zero;
                }
            }

            if (nextMoveDirection == Vector2Int.zero && isAffectedByGravity &&
                _blockManager.GravityDirection != Vector2Int.zero && IsDirectionFree(_blockManager.GravityDirection))
            {
                nextMoveDirection = _blockManager.GravityDirection;
            }

            UpdateNextMoveDirectionIndicator();
        }

        private void UpdateNextMoveDirectionIndicator()
        {
            // TODO: Animate move direction indicator
            _movementIndicator.gameObject.SetActive(false);

            if (nextMoveDirection != Vector2Int.zero)
            {
                _movementIndicator.gameObject.SetActive(true);
                _movementIndicator.rotation =
                    Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, nextMoveDirection));
            }
        }
    }
}