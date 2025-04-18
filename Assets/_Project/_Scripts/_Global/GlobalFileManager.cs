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

        void OnEnable()
        {
            GlobalEventsManager.Instance.EnemyDefeatedEventTriggered += AddEnemyOnFile;
        }

        void OnDisable()
        {
            GlobalEventsManager.Instance.EnemyDefeatedEventTriggered -= AddEnemyOnFile;
        }

        void AddEnemyOnFile(EnemyType enemyType)
        {
            switch (enemyType)
            {
                case EnemyType.MinionEasyFirst:
                    enemiesDefeatedDict[EnemyType.MinionEasyFirst] += 1;
                    break;
                case EnemyType.MinionEasySecond:
                    enemiesDefeatedDict[EnemyType.MinionEasySecond] += 1;
                    break;
                case EnemyType.BossEasyFirst:
                    enemiesDefeatedDict[EnemyType.BossEasyFirst] += 1;
                    break;
            }

            Debug.Log($"Amount of {enemyType} defeated in Dictionary: {enemiesDefeatedDict[enemyType]}");
        }
    }
}
