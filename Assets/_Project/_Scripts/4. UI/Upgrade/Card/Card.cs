using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace GoodVillageGames.Game.General.UI
{
    public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private RectTransform _visualHolderRectTrans;
        [SerializeField] private Image _shadow;
        private UIOutline _outline;

        private Vector2 _mousePosition;
        private float _tiltX;
        private float _tiltY;
        private bool _mouseHover = false;
        private Tween _rotationTween;

        private readonly float _pointerTiltMultiplier = 0.1f;
        private readonly float _maxRotationAngle = 15f;
        private readonly float _tweenDuration = 0.1f;
        private readonly Vector2 _visualDefaultSize = new(340f, 502f);
        private readonly Vector2 _visualSizeOnHover = new(374f, 552.2f);

        void Start()
        {
            _outline = GetComponentInChildren<UIOutline>();
        }

        void Update()
        {
            if (_mouseHover)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_visualHolderRectTrans, Input.mousePosition, null, out _mousePosition);
                TiltCard();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _mouseHover = true;
            _outline.enabled = true;
            _shadow.enabled = true;
            IncreaseCardSize();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _mouseHover = false;
            _outline.enabled = false;
            _shadow.enabled = false;
            DefaultCard();
        }

        void TiltCard()
        {
            Vector2 rectCenter = _visualHolderRectTrans.rect.center;
            Vector2 offset = _mousePosition - rectCenter;

            _tiltX = -offset.y * _pointerTiltMultiplier;
            _tiltY = offset.x * _pointerTiltMultiplier;

            _tiltX = Mathf.Clamp(_tiltX, -_maxRotationAngle, _maxRotationAngle);
            _tiltY = Mathf.Clamp(_tiltY, -_maxRotationAngle, _maxRotationAngle);

            Vector3 targetRotation = new(_tiltX, _tiltY, 0);

            _rotationTween?.Kill();
            _rotationTween = _visualHolderRectTrans.DOLocalRotate(targetRotation, _tweenDuration).SetEase(Ease.OutQuad);
        }

        void IncreaseCardSize()
        {
            _visualHolderRectTrans.DOSizeDelta(_visualSizeOnHover, 0.2f).SetEase(Ease.OutElastic);
        }

        void DefaultCard()
        {
            _rotationTween?.Kill();
            _visualHolderRectTrans.DOLocalRotate(Vector3.zero, 0.3f).SetEase(Ease.OutQuad);
            _visualHolderRectTrans.DOSizeDelta(_visualDefaultSize, 0.3f).SetEase(Ease.OutElastic);
        }
    }
}
