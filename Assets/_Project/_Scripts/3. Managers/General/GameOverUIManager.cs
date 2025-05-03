using TMPro;
using UnityEngine;
using TriInspector;
using System.Collections;
using System.Collections.Generic;
using GoodVillageGames.Game.Core.Global;
using GoodVillageGames.Game.Enums.Upgrades;
using GoodVillageGames.Game.DataCollection;

namespace GoodVillageGames.Game.Core.Manager
{
    public class GameOverUIManager : MonoBehaviour
    {
        [Title("Animation Settings")]
        [SerializeField] private float initialDelay = 1f;
        [SerializeField] private float elementRevealInterval = 0.2f;
        [SerializeField] private float scoreCountDuration = 2f;
        [SerializeField] private float numberRolloverSpeed = 0.05f;

        [Title("Upgrade Rarities")]
        [SerializeField] private List<TextMeshProUGUI> upgradesRaritiesTexts;

        [Title("Stats")]
        [SerializeField] private List<string> statsNames;
        [SerializeField] private List<TextMeshProUGUI> statsTexts;

        [Title("Final Score")]
        [SerializeField] private TextMeshProUGUI totalScoreText;

        [Title("Button")]
        [SerializeField] private GameObject proceedButton;

        private Dictionary<UpgradeRarity, TextMeshProUGUI> rarityTextMap;
        private Dictionary<string, TextMeshProUGUI> statTextMap;
        private GameRunData currentRunData = new();

        void Awake()
        {
            currentRunData = GlobalDataCollectorManager.Instance.CurrentRunData;
            InitializeDictionaries();
        }

        void Start() => Time.timeScale = 1;

        public void TriggerGameOverScreen()
        {
            StartCoroutine(AnimateGameOverScreen());
        }

        void InitializeDictionaries()
        {
            // Mapping Upgrades Rarities to their text components
            rarityTextMap = new Dictionary<UpgradeRarity, TextMeshProUGUI>();
            for (int i = 0; i < upgradesRaritiesTexts.Count; i++)
            {
                var rarity = (UpgradeRarity)i;
                rarityTextMap[rarity] = upgradesRaritiesTexts[i];
            }

            // Mapping Stats to their text components
            statTextMap = new Dictionary<string, TextMeshProUGUI>();
            for (int i = 0; i < statsNames.Count && i < statsTexts.Count; i++)
            {
                statTextMap[statsNames[i]] = statsTexts[i];
            }
        }

        IEnumerator AnimateGameOverScreen()
        {
            yield return new WaitForSeconds(initialDelay);

            // Animate upgrades first
            yield return StartCoroutine(AnimateUpgradeCounts());

            // Then animate stats
            yield return StartCoroutine(AnimateStats());

            // Finally animate score
            yield return StartCoroutine(AnimateFinalScore());

            proceedButton.SetActive(true);
        }

        IEnumerator AnimateUpgradeCounts()
        {
            foreach (var entry in currentRunData.UpgradesCollected)
            {
                if (rarityTextMap.TryGetValue(entry.Key, out var textElement))
                {
                    StartCoroutine(AnimateNumberRoll(textElement, entry.Value));
                    yield return new WaitForSeconds(elementRevealInterval);
                }
            }
        }

        IEnumerator AnimateStats()
        {
            foreach (var kv in statTextMap)
            {
                string statName = kv.Key;
                var textElement = kv.Value;

                if (!currentRunData.PlayerStats.TryGetValue(statName, out float value))
                    continue;

                if (statName == "MaxHealth")
                {
                    textElement.alpha = 1f;
                    int v = Mathf.RoundToInt(value);
                    textElement.text = $"{v}/{v}";
                    yield return new WaitForSeconds(elementRevealInterval);
                }
                else if (statName == "BoostRechargeRate")
                {
                    textElement.alpha = 1f;
                    textElement.text  = value.ToString("F1");
                    yield return new WaitForSeconds(elementRevealInterval);
                }
                else
                {
                    StartCoroutine(AnimateStat(textElement, value));
                    yield return new WaitForSeconds(elementRevealInterval);
                }
            }
        }


        IEnumerator AnimateStat(TextMeshProUGUI textElement, float targetValue)
        {
            // Initial hidden state
            textElement.alpha = 0;
            textElement.text = "0";

            // Fade in
            float fadeDuration = 0.5f;
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                textElement.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
                yield return null;
            }
            textElement.alpha = 1;

            // Animate number roll
            yield return StartCoroutine(AnimateNumberRoll(textElement, (int)targetValue));
        }

        IEnumerator AnimateFinalScore()
        {
            totalScoreText.alpha = 0;
            var targetScore = currentRunData.TotalRunScore;

            // Fade in
            float fadeDuration = 0.5f;
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                totalScoreText.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
                yield return null;
            }

            // Count up
            float current = 0;
            float duration = scoreCountDuration;
            float startTime = Time.time;

            while (current < targetScore)
            {
                float t = (Time.time - startTime) / duration;
                current = Mathf.Lerp(0, targetScore, t * t); // Ease out
                totalScoreText.text = Mathf.RoundToInt(current).ToString();
                yield return null;
            }

            totalScoreText.text = targetScore.ToString();
        }

        IEnumerator AnimateNumberRoll(TextMeshProUGUI textElement, int targetNumber)
        {
            int current = 0;
            textElement.text = "0";

            while (current < targetNumber)
            {
                current = Mathf.Min(targetNumber, current + Mathf.CeilToInt(targetNumber * numberRolloverSpeed));
                textElement.text = current.ToString();
                yield return null;
            }
        }
    }
}