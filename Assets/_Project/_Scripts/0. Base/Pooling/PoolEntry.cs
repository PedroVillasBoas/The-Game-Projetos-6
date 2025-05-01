using System;
using UnityEngine;
using TriInspector;
using GoodVillageGames.Game.Enums.Pooling;

namespace GoodVillageGames.Game.Core.Pooling
{
    [Serializable]
    public class PoolEntry
    {
        [Title("Pool Entry")]
        public PoolID poolId;
        public GameObject prefab;
        public PoolConfig config;
        public GameObject poolContainer;
    }
}
