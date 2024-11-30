using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Events;

namespace Sokabon.StateMachine
{
	public class CallbackWithState : StateChangeListener
	{
		public UnityEvent OnEnter;
		public UnityEvent OnExit;
		
		protected override void OnEnterEvent()
		{
			base.OnEnterEvent();
			OnEnter?.Invoke();
		}

		protected override void OnExitEvent()
		{
			base.OnExitEvent();
			OnExit?.Invoke();
		}
	}
}