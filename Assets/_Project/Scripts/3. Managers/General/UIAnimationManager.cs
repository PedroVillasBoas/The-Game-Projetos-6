using UnityEngine;
using DG.Tweening;
using System.Collections;
using static GoodVillageGames.Game.Enums.Enums;
using GoodVillageGames.Game.Core.ScriptableObjects;

namespace GoodVillageGames.Game.Core.Manager
{
    public class UIAnimationManager : MonoBehaviour
    {
        private UIState _uiState = UIState.NORMAL_UI;
        private Coroutine _animationsCoroutine;

        void OnAnimationAsked(Sequence sequence, SceneScriptableObject sceneSO)
        {
            _animationsCoroutine = StartCoroutine(PlayAnimationAskedRoutine(sequence, sceneSO));
            
        }
        
        private IEnumerator PlayAnimationAskedRoutine(Sequence sequence, SceneScriptableObject sceneSO)
        {
            _uiState = UIState.PLAYING_UI_ANIM;

            EventsManager.Instance.AnimationTriggerEvent(_uiState);
            sequence.Play();
            yield return new WaitForSeconds(sequence.Duration());

            if (sceneSO != null)
                EventsManager.Instance.ChangeSceneTriggerEvent(sceneSO);

            _uiState = UIState.NORMAL_UI;
            EventsManager.Instance.AnimationTriggerEvent(_uiState);
        }

        void OnEnable()
        {
            EventsManager.Instance.OnAnimationAskedEventTriggered += OnAnimationAsked;
        }

        void OnDisable()
        {
            EventsManager.Instance.OnAnimationAskedEventTriggered -= OnAnimationAsked;
        }

    }
}
