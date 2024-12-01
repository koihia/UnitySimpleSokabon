using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DG.Tweening;
using Sokabon.Audio;
using Sokabon.CommandSystem;
using Sokabon.InventorySystem;
using Sokabon.StateMachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sokabon
{
	public class Player : StateChangeListener
	{
		[SerializeField] private LayerSettings layerSettings;

		private Block _block;
        private Inventory _inventory;

		[SerializeField] private TurnManager _turnManager;
		[SerializeField] private BlockManager blockManager;
		[SerializeField] private SfxManager sfxManager;
		
		[SerializeField] private AudioClip[] validInputSounds;
		[SerializeField] private AudioClip[] invalidInputSounds;

		private bool _canMove = true;
		public bool IsDead { get; private set; }
		
		private Queue<Func<bool>> _actionQueue;

		private void Awake()
		{
			_actionQueue = new ();
			_canMove = true;
			_block = GetComponent<Block>();
			_inventory = GetComponent<Inventory>();

			var blocks = FindObjectsOfType<Block>();
			foreach (var block in blocks)
			{
				if (block == _block)
				{
					continue;
				}
				
				block.AtNewPositionEvent += CheckForDeath;
			}
			
			//We have a dependency on TurnManager.
			//TurnManager has not implemented the singleton pattern in this example. This is a clear weak link in this project as an example project.
			//Mostly that's because I don't want to demonstrate the singleton pattern here....
			//TurnManager doesn't need to be a monobehaviour. We, the Player, could just have one. turnManager = new TurnManager(); It could also be a ScriptableObject. ScriptableObject-Instead-of-singletons data approach is something I am partial to, but it's got all sorts of quirks, to put it nicely.
			//I like to keep my player pretty bare, and move logic away from them to managers that can just hang out. A) it makes working with AI, game-state-search (solving), or such where we may not have a proper "player" easier, and B) it makes destroying the player for animations and fade-outs and ragdolls and cutscenes and such easier.
			if (_turnManager == null)
			{
				Debug.LogWarning("Player object needs TurnManager set, or TurnManager not found in scene. Searching for one.",gameObject);
				_turnManager = GameObject.FindObjectOfType<TurnManager>();
			}

			if (blockManager == null)
			{
				Debug.LogWarning(
					"Player object needs BlockManager set, or BlockManager not found in scene. Searching for one.",
					gameObject);
				blockManager = FindObjectOfType<BlockManager>();
			}
			
			if (sfxManager == null)
			{
				Debug.LogWarning(
					"Player object needs SfxManager set, or SfxManager not found in scene. Searching for one.",
					gameObject);
				sfxManager = FindObjectOfType<SfxManager>();
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			TurnManager.AfterTurnExecutedEvent += blockManager.UpdateBlocksNextMoveDirection;
			TurnManager.AfterUndoEvent += blockManager.UpdateBlocksNextMoveDirection;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			TurnManager.AfterTurnExecutedEvent -= blockManager.UpdateBlocksNextMoveDirection;
			TurnManager.AfterUndoEvent -= blockManager.UpdateBlocksNextMoveDirection;
		}

		private void Start()
		{
			// Note: This is a bit of a hack. We need to update the blocks next move direction after the level is loaded
			// in Start() because Block initiates its BlockManager in Awake() so we can't do this in OnEnable().
			blockManager.UpdateBlocksNextMoveDirection();
		}

		private void CheckForDeath(bool isReplay)
		{
			if (isReplay && !IsDead)
			{
				return;
			}
			Collider2D col = Physics2D.OverlapCircle(_block.rb.position, 0.3f, layerSettings.blockLayerMask);
			IsDead = col?.GetComponent<Block>() != null;
		}

		protected override void OnEnterEvent()
		{
			_canMove = true;
		}

		protected override void OnExitEvent()
		{
			_canMove = false;
		}

		private void Update()
		{
			if (!_canMove)
			{
				return;//cant move.
			}

			//Todo: Joystick support. Switch to new input system.
			if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
			{
				_actionQueue.Enqueue(() => blockManager.PlayerTryMove(_block, Vector2Int.up));
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
			{
				_actionQueue.Enqueue(() => blockManager.PlayerTryMove(_block, Vector2Int.down));
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
			{
				_actionQueue.Enqueue(() => blockManager.PlayerTryMove(_block, Vector2Int.left));
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
			{
				_actionQueue.Enqueue(() => blockManager.PlayerTryMove(_block, Vector2Int.right));
			}
			else if (Input.GetKeyDown(KeyCode.Space))
			{
				_actionQueue.Enqueue(() => blockManager.PlayerTryMove(_block, Vector2Int.zero));
			}
			else if (Input.GetKeyDown(KeyCode.Z))
			{
				_actionQueue.Enqueue(() =>
				{
					_turnManager.Undo();
					return true;
				});
			}
			else if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || 
			                      Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4) || 
			                      Input.GetKeyDown(KeyCode.Alpha5))
            {
                int itemIndex = -1;
                if (Input.GetKeyDown(KeyCode.Alpha1)) itemIndex = 0;
                else if (Input.GetKeyDown(KeyCode.Alpha2)) itemIndex = 1;
                else if (Input.GetKeyDown(KeyCode.Alpha3)) itemIndex = 2;
                else if (Input.GetKeyDown(KeyCode.Alpha4)) itemIndex = 3;
                else if (Input.GetKeyDown(KeyCode.Alpha5)) itemIndex = 4;

                _actionQueue.Enqueue(() => TryPutItem(itemIndex));
            }

			if (!DOTween.IsTweening("OnBoardMovement") && _actionQueue.Count > 0)
			{
				var action = _actionQueue.Dequeue();
				if (!action())
				{
					sfxManager.PlayRandom(invalidInputSounds);
					if (!DOTween.IsTweening(_block.sprite.transform))
					{
						_block.sprite.transform.DOShakePosition(0.15f, 0.1f);
					}
					
					_actionQueue.Clear();
				}
				else
				{
					sfxManager.PlayRandom(validInputSounds);
				}
			}
			
		}
		
		private bool TryPutItem(int itemIndex)
		{
			if (!_inventory.IsItemIndexValid(itemIndex))
			{
				return false;
			}
			var col = Physics2D.OverlapCircle(transform.position, 0.3f, layerSettings.triggerLayerMask);
			if (col is not null)
			{
				return false;
			}
			
			_turnManager.ExecuteCommand(new PutItem(_inventory, itemIndex));
			return true;
		}
	}
}
