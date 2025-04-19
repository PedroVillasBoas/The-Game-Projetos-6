using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using GoodVillageGames.Game.Core.Util;

namespace GoodVillageGames.Game.General.UI
{
    public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerClickHandler
    {
        [SerializeField] private RectTransform _visualHolderRectTrans;
        [SerializeField] private Image _shadow;
        private UIOutline _outline;

        private Vector2 _mousePosition;
        private bool _mouseHover = false;

        // Tilt parameters
        private readonly float _pointerTiltMultiplier = 0.1f;
        private readonly float _maxRotationAngle = 15f;
        private float _tiltVelocityX;
        private float _tiltVelocityY;
        private const float TILT_SMOOTH_TIME = 0.1f;

        // Shadow parameters
        private readonly float _shadowTiltMultiplier = 0.5f;
        private readonly float _maxShadowAngle = 7.5f;

        // Size animation parameters
        private readonly Vector2 _visualDefaultSize = new(340f, 502f);
        private readonly Vector2 _visualSizeOnHover = new(374f, 552.2f);
        private Coroutine _sizeAnimationCoroutine;
        private Coroutine _rotationResetCoroutine;

        void Start()
        {
            _outline = GetComponentInChildren<UIOutline>();
            _visualHolderRectTrans.sizeDelta = _visualDefaultSize;
            _shadow.rectTransform.localRotation = Quaternion.identity;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _mouseHover = true;
            _outline.enabled = true;
            _shadow.enabled = true;
            StartSizeAnimation(_visualSizeOnHover, 0.2f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _mouseHover = false;
            _outline.enabled = false;
            _shadow.enabled = false;
            StartSizeAnimation(_visualDefaultSize, 0.3f);
            StartRotationReset();
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (_mouseHover)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _visualHolderRectTrans,
                    eventData.position,
                    null,
                    out _mousePosition
                );
                UpdateTilt();
            }
        }

        void UpdateTilt()
        {
            Vector2 rectCenter = _visualHolderRectTrans.rect.center;
            Vector2 offset = _mousePosition - rectCenter;

            float targetTiltX = Mathf.Clamp(-offset.y * _pointerTiltMultiplier, -_maxRotationAngle, _maxRotationAngle);
            float targetTiltY = Mathf.Clamp(offset.x * _pointerTiltMultiplier, -_maxRotationAngle, _maxRotationAngle);

            // Smooth damp (rotation)
            float newX = Mathf.SmoothDampAngle(_visualHolderRectTrans.localEulerAngles.x, targetTiltX, ref _tiltVelocityX, TILT_SMOOTH_TIME, Mathf.Infinity, Time.unscaledDeltaTime);
            float newY = Mathf.SmoothDampAngle(_visualHolderRectTrans.localEulerAngles.y, targetTiltY, ref _tiltVelocityY, TILT_SMOOTH_TIME, Mathf.Infinity, Time.unscaledDeltaTime);

            // Card rotation
            _visualHolderRectTrans.localRotation = Quaternion.Euler(newX, newY, 0);

            // Inverse shadow rotation
            float shadowX = Mathf.Clamp(-newX * _shadowTiltMultiplier, -_maxShadowAngle, _maxShadowAngle);
            float shadowY = Mathf.Clamp(-newY * _shadowTiltMultiplier, -_maxShadowAngle, _maxShadowAngle);
            _shadow.rectTransform.localRotation = Quaternion.Euler(shadowX, shadowY, 0);
        }

        void StartSizeAnimation(Vector2 targetSize, float duration)
        {
            if (_sizeAnimationCoroutine != null)
                StopCoroutine(_sizeAnimationCoroutine);

            _sizeAnimationCoroutine = StartCoroutine(AnimateSize(targetSize, duration));
        }

        IEnumerator AnimateSize(Vector2 targetSize, float duration)
        {
            Vector2 startSize = _visualHolderRectTrans.sizeDelta;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                // Elastic easing
                t = Mathf.Sin(t * Mathf.PI * 0.5f);
                _visualHolderRectTrans.sizeDelta = Vector2.Lerp(startSize, targetSize, t);
                yield return null;
            }

            _visualHolderRectTrans.sizeDelta = targetSize;
        }

        void StartRotationReset()
        {
            if (_rotationResetCoroutine != null)
                StopCoroutine(_rotationResetCoroutine);

            _rotationResetCoroutine = StartCoroutine(ResetRotation());
        }

        IEnumerator ResetRotation()
        {
            Quaternion startCardRotation = _visualHolderRectTrans.localRotation;
            Quaternion startShadowRotation = _shadow.rectTransform.localRotation;
            float elapsed = 0f;
            const float duration = 0.3f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                // Quadratic easing out
                t = 1 - (1 - t) * (1 - t);
                
                // Interpolating both card and shadow rotations
                _visualHolderRectTrans.localRotation = Quaternion.Slerp(startCardRotation, Quaternion.identity, t);
                _shadow.rectTransform.localRotation = Quaternion.Slerp(startShadowRotation, Quaternion.identity, t);
                yield return null;
            }

            // Just to make sure...
            _visualHolderRectTrans.localRotation = Quaternion.identity;
            _shadow.rectTransform.localRotation = Quaternion.identity;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // Lock Card Selection
            // Lock Card Outline to enabled
            // Lock Card Size as if it was the hover size
            // Disable the OnHoverExit if card is the one selected
            // Enable the Button to Confirm Selection
        }
    }
}