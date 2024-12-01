﻿using System;
using System.Collections.Generic;
using Sokabon.CommandSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sokabon
{
    public class BlockManager : MonoBehaviour
    {
        private Vector2Int _gravityDirection = Vector2Int.zero;
        public event Action<Vector2Int> OnGravityDirectionChanges;

        public Vector2Int GravityDirection
        {
            get => _gravityDirection;
            set
            {
                if (_gravityDirection != value)
                {
                    _gravityDirection = value;
                    OnGravityDirectionChanges?.Invoke(_gravityDirection);
                }
            }
        }

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
        
        public void Start()
        {
            OnGravityDirectionChanges?.Invoke(GravityDirection);
        }

        public bool PlayerTryMove(Block playerBlock, Vector2Int direction)
        {
            // We need to execute the PlayerNoOp command before calling TrySetNextMoveDirection because PlayerNoOp
            // does not have an animation, and it calls AfterTurnExecutedEvent immediately, which invokes
            // UpdateBlocksNextMoveDirection and sets the nextMoveDirection of the playerBlock and the pushed
            // block to Vector2Int.zero.
            // We execute the PlayerNoOp command no matter what because we want to tick the blocks on player move,
            // even if the input is invalid.
            turnManager.ExecuteCommand(new PlayerNoOp());

            var isMoveSuccessful = TrySetNextMoveDirection(playerBlock, direction);
            if (playerBlock.nextMoveDirection != Vector2Int.zero)
            {
                turnManager.ExecuteCommand(new Move(playerBlock, playerBlock.nextMoveDirection, false));
            }

            // Tick the blocks on player move
            foreach (var block in _blocks)
            {
                if (block == playerBlock)
                {
                    continue;
                }

                if (block.nextMoveDirection != Vector2Int.zero)
                {
                    turnManager.ExecuteCommand(new Move(block, block.nextMoveDirection, false));
                }
            }

            return isMoveSuccessful;
        }

        public void UpdateBlocksNextMoveDirection()
        {
            foreach (var block in _blocks)
            {
                block.UpdateNextMoveDirection();
            }
        }

        private bool TrySetNextMoveDirection(Block playerBlock, Vector2Int direction)
        {
            if (direction == Vector2Int.zero)
            {
                return true;
            }

            if (playerBlock.nextMoveDirection != Vector2Int.zero || GravityDirection + direction == Vector2Int.zero)
            {
                return false;
            }

            if (playerBlock.IsDirectionFree(direction))
            {
                playerBlock.nextMoveDirection = direction;
                return true;
            }

            Block pushedBlock = playerBlock.BlockInDirection(direction);
            if (pushedBlock is not null && pushedBlock.IsDirectionFree(direction) &&
                (pushedBlock.nextMoveDirection == Vector2Int.zero || pushedBlock.nextMoveDirection == direction ||
                 pushedBlock.BlockInDirection(direction)?.nextMoveDirection == direction))
            {
                playerBlock.nextMoveDirection = direction;
                pushedBlock.nextMoveDirection = direction;
                return true;
            }

            return false;
        }
    }
}