using UnityEngine;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.Core.Manager
{
    public class ExpSpawnManager : MonoBehaviour
    {
        public static ExpSpawnManager Instance { get; private set; }

        void Awake()
        {
            // Singleton
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public void SpawnEXP(EnemyType enemyType)
        {
            
        }
    }
}
