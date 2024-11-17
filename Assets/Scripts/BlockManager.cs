using System;
using System.Collections.Generic;
using Sokabon.CommandSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sokabon
{
    public class BlockManager : MonoBehaviour
    {
        public Vector2Int gravityDirection = Vector2Int.zero;
        [SerializeField] private TurnManager turnManager;

        private Block[] _blocks;

        private void Awake()
        {
            _blocks = FindObjectsOfType<Block>();

            //We have a dependency on TurnManager.
            //TurnManager has not implemented the singleton pattern in this example. This is a clear weak link in this project as an example project.
            //Mostly that's because I don't want to demonstrate the singleton pattern here....
            //TurnManager doesn't need to be a monobehaviour. We, the Player, could just have one. turnManager = new TurnManager(); It could also be a ScriptableObject. ScriptableObject-Instead-of-singletons data approach is something I am partial to, but it's got all sorts of quirks, to put it nicely.
            //I like to keep my player pretty bare, and move logic away from them to managers that can just hang out. A) it makes working with AI, game-state-search (solving), or such where we may not have a proper "player" easier, and B) it makes destroying the player for animations and fade-outs and ragdolls and cutscenes and such easier.
            if (turnManager == null)
            {
                Debug.LogWarning(
                    "Player object needs TurnManager set, or TurnManager not found in scene. Searching for one.",
                    gameObject);
                turnManager = FindObjectOfType<TurnManager>();
            }
        }

        public bool PlayerTryMove(Block playerBlock, Vector2Int direction)
        {
            var isMoveSuccessful = TryMove(playerBlock, direction);
            if (!isMoveSuccessful)
            {
                return false;
            }
            
            foreach (var block in _blocks)
            {
                if (block.IsAnimating)
                {
                    continue;
                }

                // tick inertia blocks
                if (block.isAffectedByInertia)
                {
                    if (block.previousMoveDirection != Vector2Int.zero && block.IsDirectionFree(block.previousMoveDirection))
                    {
                        turnManager.ExecuteCommand(new Move(block, block.previousMoveDirection, false));
                        continue;
                    }
                    
                    // Reset inertia direction if it's blocked
                    // It can then be affected by gravity, so we don't continue here
                    turnManager.ExecuteCommand(new ResetInertiaDirection(block));
                }
                
                // tick gravity blocks
                if (block.isAffectedByGravity && gravityDirection != Vector2Int.zero && block.IsDirectionFree(gravityDirection))
                {
                    turnManager.ExecuteCommand(new Move(block, gravityDirection, false));
                }
            }

            return true;
        }

        private bool TryMove(Block playerBlock, Vector2Int direction)
        {
            if (direction == Vector2Int.zero)
            {
                turnManager.ExecuteCommand(new PlayerNoOp());
                return true;
            }

            if ((gravityDirection != Vector2Int.zero && playerBlock.isAffectedByGravity &&
                 (gravityDirection + direction == Vector2Int.zero || playerBlock.IsDirectionFree(gravityDirection))) || 
                playerBlock.isAffectedByInertia && playerBlock.previousMoveDirection != Vector2Int.zero)
            {
                return false;
            }

            if (playerBlock.IsDirectionFree(direction))
            {
                turnManager.ExecuteCommand(new Move(playerBlock, direction, true));
                return true;
            }

            Block pushedBlock = playerBlock.BlockInDirection(direction);
            if (pushedBlock != null && pushedBlock.IsDirectionFree(direction) &&
                (pushedBlock.isAffectedByInertia && pushedBlock.previousMoveDirection == direction ||
                 !pushedBlock.isAffectedByGravity || gravityDirection == Vector2Int.zero ||
                 !pushedBlock.IsDirectionFree(gravityDirection)))
            {
                turnManager.ExecuteCommand(new PushBlock(playerBlock, pushedBlock, direction));
                return true;
            }

            return false;
        }
    }
}