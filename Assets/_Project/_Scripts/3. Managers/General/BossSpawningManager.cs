using UnityEngine;
using System.Collections;
using GoodVillageGames.Game.Enums.Pooling;
using GoodVillageGames.Game.Core.MobSpawning;

namespace GoodVillageGames.Game.Core.Manager
{
    public class BossSpawningManager : MobSpawn
    {
        protected override void BeginSpawnCoroutine()
        {
            throw new System.NotImplementedException();
        }

        protected override PoolID GetRandomMobPool()
        {
            throw new System.NotImplementedException();
        }

        protected override IEnumerator SpawnRoutine()
        {
            throw new System.NotImplementedException();
        }

        protected override void SpawnWave(int mobAmountToSpawn)
        {
            throw new System.NotImplementedException();
        }
    }
}