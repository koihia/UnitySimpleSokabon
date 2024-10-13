using UnityEngine;

namespace Sokabon.Trigger
{
    public abstract class Trigger : MonoBehaviour
    {
        [SerializeField] private LayerSettings layerSettings;

        private void Awake()
        {
            //This is some fancy bitmask work.
            //Explained here: http://answers.unity.com/comments/1263936/view.html
            if (layerSettings.triggerLayerMask != (layerSettings.triggerLayerMask | (1 << gameObject.layer)))
            {
                Debug.Log(
                    "Trigger Object not on trigger layer! You probably need to set this gameObject to the appropriate layer.",
                    gameObject);
            }
        }
    }
}