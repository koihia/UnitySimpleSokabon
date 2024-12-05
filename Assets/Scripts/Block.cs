using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sokabon.CommandSystem;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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

            _movementIndicator = sprite.Find("Next Move Direction Indicator");
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

            rb.position = destination;
            
            if (instant)
            {
                sprite.position = destination;
                AtNewPositionEvent?.Invoke(isReplay);
                onComplete?.Invoke();
            }
            else
            {
                sprite.transform.DOMove(destination, movementSettings.timeToMove)
                    .SetId("OnBoardMovement")
                    .SetEase(Ease.OutBack, overshoot: 1.2f)
                    .OnComplete(() =>
                    {
                        AtNewPositionEvent?.Invoke(isReplay);
                        onComplete?.Invoke();
                    });
            }
        }

        public void Teleport(Vector3 destination, bool instant, bool isReplay, Action onComplete)
        {
            rb.position = destination;

            if (instant)
            {
                sprite.position = destination;
                AtNewPositionEvent?.Invoke(isReplay);
                onComplete?.Invoke();
            }
            else
            {
                DOTween.Sequence()
                    .SetId("OnBoardMovement")
                    .Append(sprite.transform.DOScale(Vector3.zero, movementSettings.timeToMove)
                        .SetEase(Ease.InBack))
                    .Append(sprite.transform.DOMove(destination, 0))
                    .Append(sprite.transform.DOScale(Vector3.one, movementSettings.timeToMove)
                        .SetEase(Ease.OutBack))
                    .OnComplete(() =>
                    {
                        AtNewPositionEvent?.Invoke(isReplay);
                        onComplete?.Invoke();
                    });
            }
        }

        public void SetEnabled(bool isEnabled, bool instant, Action onComplete)
        {
            rb.gameObject.SetActive(isEnabled);

            if (instant)
            {
                sprite.gameObject.SetActive(isEnabled);
                sprite.localScale = isEnabled ? Vector3.one : Vector3.zero;
                onComplete?.Invoke();
            }
            else if (isEnabled)
            {
                sprite.gameObject.SetActive(true);
                sprite.localScale = Vector3.zero;
                sprite.DOScale(Vector3.one, movementSettings.timeToMove)
                    .SetId("OnBoardMovement")
                    .SetEase(Ease.OutBack)
                    .OnComplete(() => onComplete?.Invoke());
            }
            else
            {
                sprite.DOScale(Vector3.zero, movementSettings.timeToMove)
                    .SetId("OnBoardMovement")
                    .SetEase(Ease.InBack)
                    .OnComplete(() =>
                    {
                        sprite.gameObject.SetActive(false);
                        onComplete?.Invoke();
                    });
            }
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