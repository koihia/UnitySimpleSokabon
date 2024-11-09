using System;
using UnityEngine;

namespace Sokabon.CommandSystem
{
	public class PushBlock : Command
	{
		private Block _pusher;
		private Block _pushed;
		private Vector2Int _direction;
		private int _timesToBePushed;

		public PushBlock(Block pusher, Block pushed, Vector2Int direction, int timesToBePushed)
		{
			_pusher = pusher;
			_pushed = pushed;
			_direction = direction;
			_timesToBePushed = timesToBePushed;
			IsPlayerInput = true;
		}

		public override void Execute(Action onComplete)
		{
			_pusher.MoveInDirection(_direction, false, onComplete);
			_pushed.MoveInDirection(_timesToBePushed * _direction, false, onComplete);
		}

		public override void Undo(Action onComplete)
		{
			_pusher.MoveInDirection(-_direction, true, onComplete);
			_pushed.MoveInDirection(-(_timesToBePushed * _direction), false, onComplete);
		}
	}
}