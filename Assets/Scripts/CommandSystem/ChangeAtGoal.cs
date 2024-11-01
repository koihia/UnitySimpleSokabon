using System;
using Sokabon.Trigger;
using UnityEngine;

namespace Sokabon.CommandSystem
{
    public class ChangeAtGoal : Command
    {
        private readonly bool _newAtGoal;
        private readonly TriggerTargetGoal _triggerTargetGoal;
        private readonly SpriteRenderer _spriteRenderer;
        private readonly Color _defaultColor;
        private readonly Color _atGoalColor;

        public ChangeAtGoal(bool newAtGoal, TriggerTargetGoal triggerTargetGoal, SpriteRenderer spriteRenderer)
        {
            _newAtGoal = newAtGoal;
            _triggerTargetGoal = triggerTargetGoal;
            _spriteRenderer = spriteRenderer;
            _defaultColor = _spriteRenderer.color;
            _atGoalColor = Color.Lerp(_defaultColor, Color.black, 0.5f);
        }
        
        public override void Execute(Action onComplete)
        {
            if (_newAtGoal)
            {
                SetAtGoal(onComplete);
            }
            else
            {
                SetNotAtGoal(onComplete);
            }
        }

        public override void Undo(Action onComplete)
        {
            if (_newAtGoal)
            {
                SetNotAtGoal(onComplete);
            }
            else
            {
                SetAtGoal(onComplete);
            }
        }

        public void SetAtGoal(Action onComplete)
        {
            _spriteRenderer.color = _atGoalColor;
            _triggerTargetGoal.AtGoal = true;
            onComplete?.Invoke();
        }

        public void SetNotAtGoal(Action onComplete)
        {
            _spriteRenderer.color = _defaultColor;
            _triggerTargetGoal.AtGoal = false;
            onComplete?.Invoke();
        }
    }
}