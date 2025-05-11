using UnityEngine;
using System.Collections.Generic;
using GoodVillageGames.Game.Enums.Enemy;
using GoodVillageGames.Game.Enums.Pooling;

namespace GoodVillageGames.Game.Core.Manager
{
    public class ExpSpawnManager : MonoBehaviour
    {
        public static ExpSpawnManager Instance { get; private set; }

        private Dictionary<EnemyType, PoolID> expPoolsDict;

        void Awake()
        {
            // Singleton
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            InitializeEXPDictionary();
        }

        void InitializeEXPDictionary()
        {
            expPoolsDict = new Dictionary<EnemyType, PoolID>
            {
                // (any difficulty) -> EXP Tiny
                { EnemyType.MinionEasyFirst, PoolID.PickupEXPTiny },

                // (any difficulty) -> EXP Small
                { EnemyType.MinionEasySecond, PoolID.PickupEXPSmall },
                { EnemyType.MinionMediumFirst, PoolID.PickupEXPSmall },

                // (any difficulty) -> EXP Medium
                { EnemyType.MinionMediumSecond, PoolID.PickupEXPMedium },
                { EnemyType.MinionHardFirst, PoolID.PickupEXPMedium },
                { EnemyType.BossEasyFirst, PoolID.PickupEXPMedium },

                // (any difficulty) -> EXP Large
                { EnemyType.MinionHardSecond, PoolID.PickupEXPLarge },
                { EnemyType.BossEasySecond, PoolID.PickupEXPLarge },
                { EnemyType.BossMediumFirst, PoolID.PickupEXPLarge },

                // (any difficulty) -> EXP Gigantic
                { EnemyType.BossHardFirst, PoolID.PickupEXPGigantic },
                { EnemyType.BossMediumSecond, PoolID.PickupEXPGigantic },

                // (any difficulty) -> EXP Ginormous
                { EnemyType.BossHardSecond, PoolID.PickupEXPGinormous },
            };
        }

        public void SpawnEXP(EnemyType enemyType, Vector3 position)
        {
            if (expPoolsDict.TryGetValue(enemyType, out PoolID poolID))
            {
                if (PoolManager.Instance != null)
                {
                    GameObject exp = PoolManager.Instance.GetPooledObject(poolID);
                    exp.transform.SetPositionAndRotation(position, Quaternion.identity);
                    exp.SetActive(true);
                }
            }
            else
            {
                Debug.LogWarning($"No EXP pool mapped for enemy type: {enemyType}");
            }
        }
    }
}