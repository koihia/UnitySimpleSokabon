using UnityEngine;

namespace Sokabon.Trigger
{
    public class TriggerTargetGoal : TriggerTarget
    {
        [field: SerializeField] public TriggerGoalType GoalType { get; private set; }

        private SpriteRenderer _spriteRenderer;
        private Color _defaultColor;
        private Color _atGoalColor;
        public bool AtGoal { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _defaultColor = _spriteRenderer.color;
            _atGoalColor = Color.Lerp(_defaultColor, Color.black, 0.5f);
        }

        protected override void OnSokabonTriggerEnter(Trigger trigger)
        {
            TriggerGoal triggerGoal = trigger as TriggerGoal;
            if (triggerGoal?.GoalType != GoalType)
            {
                return;
            }
            AtGoal = true;
            _spriteRenderer.color = _atGoalColor;
        }

        protected override void OnSokabonTriggerExit(Trigger trigger)
        {
            TriggerGoal triggerGoal = trigger as TriggerGoal;
            if (triggerGoal?.GoalType != GoalType)
            {
                return;
            }
            AtGoal = false;
            _spriteRenderer.color = _defaultColor;
        }
    }
}