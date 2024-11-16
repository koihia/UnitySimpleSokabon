using System;
using UnityEngine;

namespace Sokabon.CommandSystem
{
	public class PushBlock : Command
	{
		private Block _pusher;
		private Block _pushed;
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
			if (_pushed.hasInertia)
			{
				_pushed.inertiaDirection = _direction;
			}
			
			_pusher.MoveInDirection(_direction, false, onComplete);
			_pushed.MoveInDirection(_direction, false, onComplete);
		}

		public override void Undo(Action onComplete)
		{
			if (_pushed.hasInertia)
			{
				_pushed.inertiaDirection = Vector2Int.zero;
			}
			
			_pusher.MoveInDirection(-_direction, true, onComplete);
			_pushed.MoveInDirection(- _direction, false, onComplete);
		}
	}
}