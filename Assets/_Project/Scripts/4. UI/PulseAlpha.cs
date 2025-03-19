using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace GoodVillageGames.Game.General.UI
{
    public class PulseAlpha : MonoBehaviour
    {
        private Image _image;
        private Sequence _sequence;

        void Awake()
        {
            _image = GetComponent<Image>();
        }

        void Start()
        {
            _sequence = DOTween.Sequence();
            _sequence.Append(_image.DOFade(1, 1f)).SetLoops(-1, LoopType.Yoyo);
            _sequence.Play();
        }

        void OnDestroy()
        {
            _sequence?.Kill();
        }
    }
}
