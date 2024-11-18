using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Sokabon.UI
{
    public class UIGravityIndicator : MonoBehaviour
    {
        [SerializeField] private Image arrowIndicator; 
        [SerializeField] private Image noneIndicator;
        private BlockManager _blockManager;
        private bool _animating;

        private void Awake()
        {
            _blockManager = FindObjectOfType<BlockManager>();
            if (_blockManager is null)
            {
                Debug.LogError("BlockManager not found in scene");
            }
        }

        private void OnEnable()
        {
            _blockManager.OnGravityDirectionChanges += UpdateOnGravityIndicator;
        }

        private void OnDisable()
        {
            _blockManager.OnGravityDirectionChanges -= UpdateOnGravityIndicator;
        }

        private void UpdateOnGravityIndicator(Vector2Int gravityDirection)
        {
            if (gravityDirection != Vector2Int.zero)
            {
                arrowIndicator.enabled = true;
                noneIndicator.enabled = false;
                StartCoroutine(AnimateMove(gravityDirection));
            }
            else
            {
                arrowIndicator.enabled = false;
                noneIndicator.enabled = true;
            }
        }
        
        private IEnumerator AnimateMove(Vector2Int targetDirection)
        {
            while (_animating)
            {
                yield return null;
            }
            _animating = true;
            
            var start = arrowIndicator.transform.rotation;
            var target = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, targetDirection));
            var t = 0f;
            while (t < 1)
            {
                // hardcode time to move for now
                t += Time.deltaTime / 0.1f;
                // use linear interpolation to rotate the arrow fow now
                arrowIndicator.transform.rotation = Quaternion.Lerp(start, target, t);
                yield return null;
            }

            arrowIndicator.transform.rotation = target;
            _animating = false;
        }
    }
}