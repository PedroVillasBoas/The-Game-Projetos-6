using UnityEngine;
using DG.Tweening;
using System.Collections;
using static GoodVillageGames.Game.Enums.Enums;
using GoodVillageGames.Game.Core.ScriptableObjects;
using GoodVillageGames.Game.Core.Global;

namespace GoodVillageGames.Game.Core.Manager
{
    public class UIAnimationManager : MonoBehaviour
    {
        private UIState _uiState = UIState.NORMAL_UI;
        private Coroutine _animationsCoroutine;
        private AnimationTransitionID _animationTransitionID;

        void OnAnimationAsked(Sequence sequence, AnimationTransitionID animationTransitionID, SceneScriptableObject sceneSO)
        {
            _animationTransitionID = animationTransitionID;
            _animationsCoroutine = StartCoroutine(PlayAnimationAskedRoutine(sequence, sceneSO));
        }
        
        private IEnumerator PlayAnimationAskedRoutine(Sequence sequence, SceneScriptableObject sceneSO)
        {
            _uiState = UIState.PLAYING_UI_ANIM;
            
            GlobalEventsManager.Instance.AnimationTriggerEvent(_uiState);
            sequence.Play();
            yield return new WaitForSeconds(sequence.Duration());

            if (sceneSO != null)
                EventsManager.Instance.ChangeSceneTriggerEvent(sceneSO);

            EventsManager.Instance.TriggerEvent("Stop"); // UI Particles to Stop Emmiting
            _uiState = UIState.NORMAL_UI;
            GlobalEventsManager.Instance.AnimationTriggerEvent(_uiState);
        }

    }
}
