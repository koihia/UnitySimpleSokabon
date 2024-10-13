using UnityEngine;

namespace Sokabon.Trigger
{
    public class TriggerGoal : Trigger
    {
        [field: SerializeField] public TriggerGoalType GoalType { get; private set; }
    }
}