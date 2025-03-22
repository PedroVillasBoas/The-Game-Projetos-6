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


        void Start()
        {
            EventsManager.Instance.OnAnimationAskedEventTriggered += OnAnimationAsked;
        }

        void OnAnimationAsked(Sequence sequence, SceneScriptableObject sceneSO)
        {
            _animationsCoroutine = StartCoroutine(PlayAnimationAskedRoutine(sequence, sceneSO));
            
        }
        
        private IEnumerator PlayAnimationAskedRoutine(Sequence sequence, SceneScriptableObject sceneSO)
        {
            _uiState = UIState.PLAYING_UI_ANIM;

            EventsManager.Instance.AnimationTriggerEvent(_uiState);
            sequence.Play();
            Debug.Log($"Tempo da Sequence é de: {sequence.Duration()}");
            yield return sequence.WaitForCompletion();

            if (sceneSO != null)
                EventsManager.Instance.ChangeSceneTriggerEvent(sceneSO);

            _uiState = UIState.NORMAL_UI;
            EventsManager.Instance.AnimationTriggerEvent(_uiState);
        }
            
        void OnDisable()
        {
            EventsManager.Instance.OnAnimationAskedEventTriggered -= OnAnimationAsked;
        }

    }
}
