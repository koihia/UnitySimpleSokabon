using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Sokabon.UI
{
    public class UIOverlay : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private RectTransform window;
        [SerializeField] private float animationDuration;
        
        public Action OnOpenAnimStart;
        public Action OnOpenAnimEnd;
        
        private Sequence _sequence;
        
        private void Awake()
        {
            background.gameObject.SetActive(false);
            window.gameObject.SetActive(false);
        }

        public void Open()
        {
            OnOpenAnimStart?.Invoke();
            
            if (_sequence.IsActive())
            {
                _sequence.Complete();
            }
            _sequence = DOTween.Sequence();
            _sequence.AppendCallback(() =>
                {
                    background.gameObject.SetActive(true);
                    var color = background.color;
                    color.a = 0;
                    background.color = color;

                    window.gameObject.SetActive(true);
                    window.anchoredPosition = new Vector2(0, -Screen.height);
                })
                .Append(background.DOFade(0.5f, animationDuration).SetEase(Ease.OutExpo))
                .Join(window.DOAnchorPosY(0, animationDuration).SetEase(Ease.OutBack, overshoot: 1.2f))
                .AppendCallback(() => OnOpenAnimEnd?.Invoke());
        }

        public void Close()
        {
            if (_sequence.IsActive())
            {
                _sequence.Kill();
            }
            _sequence = DOTween.Sequence();
            _sequence.Append(background.DOFade(0f, animationDuration).SetEase(Ease.InExpo))
                .Join(window.DOAnchorPosY(-Screen.height, animationDuration).SetEase(Ease.InBack, overshoot: 1.2f))
                .AppendCallback(() =>
                {
                    background.gameObject.SetActive(false);
                    window.gameObject.SetActive(false);
                });
        }
    }
}