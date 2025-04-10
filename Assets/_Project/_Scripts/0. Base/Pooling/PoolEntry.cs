using System;
using UnityEngine;
using TriInspector;
using static GoodVillageGames.Game.Enums.Enums;

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
