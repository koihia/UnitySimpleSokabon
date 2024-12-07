using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sokabon.UI
{
    public class UIVictoryPanel : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private TurnManager turnManager;
        [SerializeField] private UIOverlay uiOverlay;
        [SerializeField] private TextMeshProUGUI movesText;
        [SerializeField] private TextMeshProUGUI bestMovesText;

        [SerializeField] private Image star1;
        [SerializeField] private Image star2;
        [SerializeField] private Image star3;

        private void Awake()
        {
            levelManager ??= FindObjectOfType<LevelManager>();
            uiOverlay ??= GetComponent<UIOverlay>();
            turnManager ??= FindObjectOfType<TurnManager>();
            
            star1.rectTransform.localScale = Vector3.zero;
            star2.rectTransform.localScale = Vector3.zero;
            star3.rectTransform.localScale = Vector3.zero;
        }

        private void OnEnable()
        {
            uiOverlay.OnOpenAnimStart += SetTexts;
            uiOverlay.OnOpenAnimEnd += ShowStars;
        }

        private void OnDisable()
        {
            uiOverlay.OnOpenAnimStart -= SetTexts;
            uiOverlay.OnOpenAnimEnd -= ShowStars;
        }
        
        private void SetTexts()
        {
            // Ensure that the best moves are updated before showing them
            GameDataManager.UpdateBestMoves(levelManager.CurrentLevel, turnManager.TurnCount);
            
            movesText.text = $"Moves: {turnManager.TurnCount}";
            bestMovesText.text = $"Your best: {GameDataManager.GetBestMoves(levelManager.CurrentLevel)}";
        }

        private void ShowStars()
        {
            star1.rectTransform.localScale = Vector3.zero;
            star2.rectTransform.localScale = Vector3.zero;
            star3.rectTransform.localScale = Vector3.zero;

            var starsCount = GameDataManager.GetStarsCount(levelManager.CurrentLevel, turnManager.TurnCount);
            var sequence = DOTween.Sequence();
            if (starsCount >= 1)
            {
                sequence.Append(star1.rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));
            }

            if (starsCount >= 2)
            {
                sequence.Insert(0.3f,
                    star2.rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));
            }

            if (starsCount >= 3)
            {
                sequence.Insert(0.6f,
                    star3.rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));
            }
        }
    }
}