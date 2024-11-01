using System;

namespace Sokabon.CommandSystem
{
	public class Command
	{
		public bool IsPlayerInput { get; protected set; }

		public virtual void Execute(Action onComplete)
		{
			onComplete?.Invoke();
		}

		public virtual void Undo(Action onComplete)
		{
			onComplete?.Invoke();
		}
	}
}