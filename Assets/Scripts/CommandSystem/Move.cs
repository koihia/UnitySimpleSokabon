using System;
using UnityEngine;

namespace Sokabon.CommandSystem
{
	public class Move : Command
	{
		private Block _block;
		private Vector2Int _direction;

		public Move(Block block, Vector2Int direction, bool isPlayerInput)
		{
			_block = block;
			_direction = direction;
			IsPlayerInput = isPlayerInput;
		}

		public override void Execute(Action onComplete)
		{
			_block.MoveInDirection(_direction, false, false, onComplete);
		}

		public override void Undo(Action onComplete)
		{
			_block.MoveInDirection(-_direction, true, true, onComplete);
		}
	}
}