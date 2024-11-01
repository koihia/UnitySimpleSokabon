using System;
using System.Collections;
using System.Collections.Generic;
using Sokabon.CommandSystem;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
	private Stack<Command> _commands;
	public static Action AfterTurnExecutedEvent;
	public static Action AfterUndoEvent;
	
	public Action<int> TurnCountChanges;//For the game as currently scoped, it would be fine for this to be static. That's usually not the case, so lets make sure the example is more widely applicable.
	public int TurnCount = 0;
	
	private void Awake()
	{
		_commands = new Stack<Command>();
	}

	private void Start()
	{
		TurnCountChanges?.Invoke(TurnCount);//Count is 0 on level load.
	}

	public void ExecuteCommand(Command command)
	{
		command.Execute(AfterTurnExecutedEvent);
		_commands.Push(command);
		if (command.IsPlayerInput)
		{
			TurnCount++;
			TurnCountChanges?.Invoke(TurnCount);
		}
	}

	public void Undo()
	{
		while (_commands.Count > 0)
		{
			var command = _commands.Pop();
			command.Undo(AfterUndoEvent);
			if (command.IsPlayerInput)
			{
				TurnCount--;
				TurnCountChanges?.Invoke(TurnCount);
				break;
			}
		}
	}
}
