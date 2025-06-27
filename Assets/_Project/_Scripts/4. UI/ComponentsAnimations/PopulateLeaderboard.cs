using System;
using System.Collections.Generic;
using GoodVillageGames.Game.Core.Global;
using GoodVillageGames.Game.DataCollection;
using UnityEngine;


namespace GoodVillageGames.Game.General.UI.Animations
{
    public class PopulateLeaderboard : MonoBehaviour
    {
        [SerializeField] private GameObject parent;
        [SerializeField] private GameObject nodePrefab;
        [SerializeField] private GameObject noScoreNodePrefab;

        public event Action OnLeaderboardTrigger;

        void Start()
        {
            OnLeaderboardTrigger += Populate;
        }

        void OnDestroy()
        {
            OnLeaderboardTrigger -= Populate;
        }

        public void LeaderboardTrigger()
        {
            OnLeaderboardTrigger.Invoke();
        }

        void Populate()
        {
            foreach (Transform child in parent.transform)
            {
                Destroy(child.gameObject);
            }

            List<LeaderboardData> leaderboardData = GlobalFileManager.Instance.GetSavedScores();

            if (leaderboardData == null || leaderboardData.Count == 0)
            {
                GameObject NoScoreNodeObject = Instantiate(noScoreNodePrefab, parent.transform);
            }

            int rank = 1;
            foreach (LeaderboardData scoreData in leaderboardData)
            {
                GameObject scoreNodeObject = Instantiate(nodePrefab, parent.transform);
                
                
                if (scoreNodeObject.TryGetComponent<LeaderboardNode>(out var nodeComponent))
                {
                    string placement = rank.ToString();
                    string runScore = scoreData.TotalRunScore.ToString();
                    string runTime = scoreData.TotalRunTime.ToString("F2") + "m";
                    string enemiesDefeated = scoreData.TotalEnemiesDefeated.ToString();

                    nodeComponent.SetNodeData(placement, runScore, runTime, enemiesDefeated);
                }
                
                rank++;
            }
        }
    }
}