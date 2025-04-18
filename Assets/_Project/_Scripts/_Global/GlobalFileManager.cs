using System.Collections.Generic;
using UnityEngine;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.Core.Global
{
    /// <summary>
    /// Manager responsable for writting the data on file for each player;
    /// </summary>
    public class GlobalFileManager : MonoBehaviour
    {
        public static GlobalFileManager Instance { get; private set; }

        private Dictionary<EnemyType, int> enemiesDefeatedDict = new();

        void Awake()
        {
            // Singleton
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            GlobalEventsManager.Instance.EnemyDefeatedEventTriggered += AddEnemyOnFile;
        }

        void OnDestroy()
        {
            GlobalEventsManager.Instance.EnemyDefeatedEventTriggered -= AddEnemyOnFile;
        }

        void AddEnemyOnFile(EnemyType enemyType)
        {
            if (!enemiesDefeatedDict.ContainsKey(enemyType))
                enemiesDefeatedDict.Add(enemyType, 0);

            enemiesDefeatedDict[enemyType]++;
            
            Debug.Log($"Amount of {enemyType} defeated: {enemiesDefeatedDict[enemyType]}");
        }
    }
}
