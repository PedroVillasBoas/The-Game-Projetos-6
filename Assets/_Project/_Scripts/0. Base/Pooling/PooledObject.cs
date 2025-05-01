using UnityEngine;
using GoodVillageGames.Game.Core.Manager;
using GoodVillageGames.Game.Enums.Pooling;

namespace GoodVillageGames.Game.Core.Pooling
{
    public class PooledObject : MonoBehaviour
    {
        private PoolID _poolId;

        public void SetPoolId(PoolID poolId)
        {
            _poolId = poolId;
        }

        public void ReturnToPool()
        {
            if (PoolManager.Instance != null)
            {
                PoolManager.Instance.ReturnPooledObject(_poolId, gameObject);
            }
            else
            {
                Debug.LogWarning("PoolManager instance not found. Destroying object.");
                Destroy(gameObject);
            }
        }
    }
}
