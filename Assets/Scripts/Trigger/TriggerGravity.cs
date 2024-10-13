using UnityEngine;

namespace Sokabon.Trigger
{
    public class TriggerGravity : Trigger
    {
        [field: SerializeField]
        public Vector2Int GravityDirection { get; private set; }
    }
}