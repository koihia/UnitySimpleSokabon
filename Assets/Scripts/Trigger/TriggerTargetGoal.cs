using Sokabon.CommandSystem;
using UnityEngine;

namespace Sokabon.Trigger
{
    public class TriggerTargetGoal : TriggerTarget
    {
        [field: SerializeField] public TriggerGoalType GoalType { get; private set; }

        private SpriteRenderer _spriteRenderer;
        public bool AtGoal { get; set; }

        protected override void Awake()
        {
            base.Awake();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void OnSokabonTriggerEnter(Trigger trigger)
        {
            TriggerGoal triggerGoal = trigger as TriggerGoal;
            if (triggerGoal?.GoalType != GoalType)
            {
                return;
            }
            
            _turnManager.ExecuteCommand(new ChangeAtGoal(true, this, _spriteRenderer));
        }

        protected override void OnSokabonTriggerExit(Trigger trigger)
        {
            TriggerGoal triggerGoal = trigger as TriggerGoal;
            if (triggerGoal?.GoalType != GoalType)
            {
                return;
            }
            
            _turnManager.ExecuteCommand(new ChangeAtGoal(false, this, _spriteRenderer));
        }
    }
}