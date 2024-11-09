using Sokabon.CommandSystem;
using UnityEngine;

namespace Sokabon.Trigger
{
    public class TriggerTargetGoal : TriggerTarget
    {
        [field: SerializeField] public TriggerGoalType GoalType { get; private set; }

        private SpriteRenderer _spriteRenderer;
        private Color _defaultColor;
        public bool AtGoal { get; set; }

        protected override void Awake()
        {
            base.Awake();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _defaultColor = _spriteRenderer.color;
        }

        protected override void OnSokabonTriggerEnter(Trigger trigger)
        {
            TriggerGoal triggerGoal = trigger as TriggerGoal;
            if (triggerGoal?.GoalType != GoalType)
            {
                return;
            }
            
            _turnManager.ExecuteCommand(new ChangeAtGoal(true, this, _spriteRenderer, _defaultColor));
            _soundManager?.PlayOnGoalSound();
        }

        protected override void OnSokabonTriggerExit(Trigger trigger)
        {
            TriggerGoal triggerGoal = trigger as TriggerGoal;
            if (triggerGoal?.GoalType != GoalType)
            {
                return;
            }
            
            _turnManager.ExecuteCommand(new ChangeAtGoal(false, this, _spriteRenderer, _defaultColor));
        }
    }
}