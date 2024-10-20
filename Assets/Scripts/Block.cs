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
        public Action AtNewPositionEvent;
        [SerializeField] private TurnManager turnManager;
        [SerializeField] private MovementSettings movementSettings;
        public bool IsAnimating => _animating; 
        private bool _animating;
        
        // TODO: This is a bit of a hack. We should probably have a separate class for gravity. 
        public static Vector2Int GravityDirection = Vector2Int.zero;
        public bool isAffectedByGravity = true;
        
        [SerializeField] private LayerSettings layerSettings;

        private void Awake()
        {
            // TODO: Singleton turnManager? Or put gravity in a separate class?
            if (turnManager == null)
            {
                Debug.LogWarning("Block object needs TurnManager set, or TurnManager not found in scene. Searching for one.",gameObject);
                turnManager = FindObjectOfType<TurnManager>();
            }
            GravityDirection = Vector2Int.zero;
        }
        
        private void LateUpdate()
        {
            // Checking gravity in LateUpdate avoids the one frame gap that cause _isAnimating to be false when it can
            // still continue falling. This is because we animate the block and set _isAnimating in coroutine.
            if (isAffectedByGravity && !IsAnimating && GravityDirection != Vector2Int.zero &&
                IsDirectionFree(GravityDirection))
            {
                Move move = new(this, GravityDirection);
                turnManager.ExecuteCommand(move);
            }
        }

        public Vector3 GetPosInDir(Vector2Int direction)
        {
            return transform.position + new Vector3(direction.x, direction.y, 0);
        }

        public void MoveInDirection(Vector2Int direction, bool instant, Action onComplete)
        {
            Vector3 destination = GetPosInDir(direction);

            if (instant)
            {
                transform.position = destination;
                AtNewPositionEvent?.Invoke();
                onComplete?.Invoke();
            }
            else
            {
                StartCoroutine(AnimateMove(destination,onComplete));
            }
        }

        public IEnumerator AnimateMove(Vector3 destination, Action onComplete)
        {
            _animating = true;
            Vector3 start = transform.position;
            float t = 0;
            while (t < 1)
            {
                t = t + Time.deltaTime/movementSettings.timeToMove;
                transform.position = Vector3.Lerp(start, destination, movementSettings.movementCurve.Evaluate(t));
                yield return null;
            }

            transform.position = destination;
            AtNewPositionEvent?.Invoke();
            onComplete?.Invoke();
            _animating = false;
        }

        public bool IsDirectionFree(Vector2Int direction)
        {
            Vector3 position = GetPosInDir(direction);
            //the ^ combines the two bitmaps with an OR (the pipe symbol) bitwise operation. 0011 and 0110 becomes 0111. 
            //It means we are checking for blocks AND solid stuff, and have the flexibility to differentiate.
            Collider2D col2D = Physics2D.OverlapCircle(position, 0.3f, layerSettings.solidLayerMask | layerSettings.blockLayerMask);
            return col2D == null;
        }

        public Block BlockInDirection(Vector2Int direction)
        {
            Vector3 position = GetPosInDir(direction);
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