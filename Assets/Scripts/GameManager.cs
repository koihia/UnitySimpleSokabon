using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sokabon;
using Sokabon.StateMachine;
using Sokabon.Trigger;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("State Machine Config")] [SerializeField]
    private StateMachine machine;

    [SerializeField] private State loseState;
    [SerializeField] private State victoryState;
    [SerializeField] private State gameplayState;
    [SerializeField] private State pauseState;
    //We need to know when a turn is done
    //We need to know all of the victory conditions
    private Player _player;
    private TriggerTargetGoal[] _goalTargets;
    private int _goalCount;
    private GameTimer _timer;

    public event Action OnWin;
    
    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _goalTargets = FindObjectsOfType<TriggerTargetGoal>();
        _goalCount = FindObjectsOfType<TriggerGoal>().Length;
        _timer = new GameTimer();
    }

    void Start()
    {
        machine.Init();
    }

    private void OnEnable()
    {
        TurnManager.AfterTurnExecutedEvent += AfterTurnExecutedOrUndo;
        TurnManager.AfterUndoEvent += AfterTurnExecutedOrUndo;
    }

    private void OnDisable()
    {
        TurnManager.AfterTurnExecutedEvent -= AfterTurnExecutedOrUndo;
        TurnManager.AfterUndoEvent -= AfterTurnExecutedOrUndo;
    }
    
    public GameTimer GetTimer()
    {
        return _timer;
    }

    public void Pause()
    {
        _timer.Pause();
        machine.SetCurrentState(pauseState);
    }

    public void UnPause()
    {
        _timer.Unpause();
        machine.SetCurrentState(gameplayState);
    }
    private void AfterTurnExecutedOrUndo()
    {
        if (!_timer.Started)
        {
            _timer.StartTimer();
        }

        CheckForLose();
        CheckForVictory();
    }

    private void CheckForLose()
    {
        if (machine.IsCurrentState(gameplayState) && _player.IsDead)
        {
            Debug.Log("We lose!");
            _timer.Stop();
            machine.SetCurrentState(loseState);
        }
        else if (machine.IsCurrentState(loseState) && !_player.IsDead)
        {
            _timer.Unpause();
            machine.SetCurrentState(gameplayState);
        }
    }

    private void CheckForVictory()
    {
        if (machine.IsCurrentState(victoryState))
        {
            return;
        }

        // Check if there are _goalCount goal targets on goals
        bool victory = _goalTargets.Count(target => target.AtGoal) >= _goalCount;
        if (victory)
        {
            Debug.Log("We win!");
            _timer.Stop();
            machine.SetCurrentState(victoryState);
            OnWin?.Invoke();
        }
    }

    private void Update()
    {
        _timer.Tick();
    }
    
    //Helper functions
    public bool IsCurrentStateGameplay()
    {
        return machine.IsCurrentState(gameplayState);
    }

    public bool IsCurrentStateVictory()
    {
        return machine.IsCurrentState(victoryState);
    }

    public bool IsPaused()
    {
        return machine.IsCurrentState(pauseState);
    }
}
