using UnityEngine;
using TriInspector;
using System.Collections.Generic;
using GoodVillageGames.Game.Enums;
using GoodVillageGames.Game.Interfaces;
using GoodVillageGames.Game.Core.Global;

namespace GoodVillageGames.Game.Core.Manager
{
    public class TutorialManager : MonoBehaviour
    {
        [Title("Tutorial Order")]
        [SerializeField] private List<GameObject> tutorialPrefabs;

        private int currentIndex = 0;
        private ITutorialStep currentStepInstance;

        void OnEnable() => GlobalEventsManager.Instance.ChangeGameStateEventTriggered += OnTutorialGameState;
        void OnDisable() => GlobalEventsManager.Instance.ChangeGameStateEventTriggered -= OnTutorialGameState;

        void OnTutorialGameState(GameState gameState)
        {
            if (gameState == GameState.Tutorial) StartTutorial();
        }

        private void StartTutorial()
        {
            currentIndex = 0;
            ShowNextStep();
        }

        private void ShowNextStep()
        {
            if (currentStepInstance != null)
            {
                currentStepInstance.OnContinue -= OnStepContinue;
                currentStepInstance.Exit();
            }

            if (currentIndex >= tutorialPrefabs.Count)
            {
                TutorialComplete();
                return;
            }

            GameObject prefab = tutorialPrefabs[currentIndex];
            GameObject go = Instantiate(prefab, transform);
            currentStepInstance = go.GetComponent<ITutorialStep>();
            currentStepInstance.OnContinue += OnStepContinue;
            currentStepInstance.Enter();
        }

        private void OnStepContinue()
        {
            currentIndex++;
            ShowNextStep();
        }

        private void TutorialComplete()
        {
            GlobalEventsManager.Instance.ChangeGameState(GameState.GameBegin);
            Debug.Log("Tutorial finished! Starting game...");
            Destroy(gameObject);
        }
    }
}
