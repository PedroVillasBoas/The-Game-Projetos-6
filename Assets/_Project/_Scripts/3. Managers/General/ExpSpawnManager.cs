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
                // Minion First (any difficulty) -> EXP Tiny
                { EnemyType.MinionEasyFirst, PoolID.PickupEXPTiny },
                { EnemyType.MinionMediumFirst, PoolID.PickupEXPTiny },
                { EnemyType.MinionHardFirst, PoolID.PickupEXPTiny },

                // Minion Second (any difficulty) -> EXP Small
                { EnemyType.MinionEasySecond, PoolID.PickupEXPSmall },
                { EnemyType.MinionMediumSecond, PoolID.PickupEXPSmall },
                { EnemyType.MinionHardSecond, PoolID.PickupEXPSmall },

                // Boss First (any difficulty) -> EXP Large
                { EnemyType.BossEasyFirst, PoolID.PickupEXPLarge },
                { EnemyType.BossMediumFirst, PoolID.PickupEXPLarge },
                { EnemyType.BossHardFirst, PoolID.PickupEXPLarge },

                // Boss Second (any difficulty) -> EXP Gigantic
                { EnemyType.BossEasySecond, PoolID.PickupEXPGigantic },
                { EnemyType.BossMediumSecond, PoolID.PickupEXPGigantic },
                { EnemyType.BossHardSecond, PoolID.PickupEXPGigantic }
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