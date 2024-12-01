using System;
using Sokabon.CommandSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sokabon.Trigger
{
    public class TriggerTargetGoal : TriggerTarget
    {
        [field: SerializeField] public TriggerGoalType GoalType { get; private set; }
        [SerializeField] private AudioClip[] goalSounds;

        private SpriteRenderer _spriteRenderer;
        private Color _defaultColor;
        public bool AtGoal { get; set; }

        protected void Start()
        {
            // Block initialize the sprite renderer in Awake, so we need to get it here
            _spriteRenderer = GetComponent<Block>().sprite.Find("Base").GetComponent<SpriteRenderer>();
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
            _SfxManager?.PlayRandom(goalSounds);
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