using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace GoodVillageGames.Game.General.UI
{
    public class FadeInAndOut : MonoBehaviour
    {
        [SerializeField] private bool _fadeIN = false; 
        [SerializeField] private bool _fadeOut = false; 
        [SerializeField] private float _length = 1f;

        private Tweener _fadeInTweener;
        private Tweener _fadeOutTweener;

        private Image _image;


        protected void FadeIN()
        {
            _fadeInTweener?.Kill();
            _fadeInTweener = _image.DOFade(1f, _length);
        }

        protected void FadeOUT()
        {
            _fadeOutTweener?.Kill();
            _fadeOutTweener = _image.DOFade(0f, _length);
        }
    }
}
