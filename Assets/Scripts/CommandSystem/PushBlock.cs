using System;
using UnityEngine;

namespace Sokabon.CommandSystem
{
	public class PushBlock : Command
	{
		private Block _pusher;
		private Block _pushed;
		private Vector2Int _oldPusherPreviousDirection;
		private Vector2Int _oldPushedPreviousDirection;
		private Vector2Int _direction;

		public PushBlock(Block pusher, Block pushed, Vector2Int direction)
		{
			_pusher = pusher;
			_pushed = pushed;
			_direction = direction;
			IsPlayerInput = true;
		}

		public override void Execute(Action onComplete)
		{
			_oldPusherPreviousDirection = _pusher.previousMoveDirection;
			_pusher.previousMoveDirection = _direction;
			_pusher.MoveInDirection(_direction, false, false, onComplete);
			_oldPushedPreviousDirection = _pushed.previousMoveDirection;
			_pushed.previousMoveDirection = _direction;
			_pushed.MoveInDirection(_direction, false, false, onComplete);
		}

		public override void Undo(Action onComplete)
		{
			_pusher.previousMoveDirection = _oldPusherPreviousDirection;
			_pusher.MoveInDirection(-_direction, true, true, onComplete);
			_pushed.previousMoveDirection = _oldPushedPreviousDirection;
			_pushed.MoveInDirection(-_direction, true, true, onComplete);
		}
	}
}