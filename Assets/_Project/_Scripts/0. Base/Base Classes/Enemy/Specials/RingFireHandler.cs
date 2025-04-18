using UnityEngine;
using GoodVillageGames.Game.Core.Manager;
using GoodVillageGames.Game.Core.Projectiles;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.Handlers
{
    public class RingFireHandler : MonoBehaviour
    {
        [SerializeField] private PoolID poolID;
        [SerializeField] private int count = 12;

        public void FireRing(Vector3 origin, float damage)
        {
            float step = 360f / count;
            for (int i = 0; i < count; i++)
            {
                var rot = Quaternion.Euler(0, 0, i * step);
                var proj = PoolManager.Instance.GetPooledObject(poolID);

                if (proj == null) continue;

                proj.transform.SetPositionAndRotation(origin, rot);
                
                if (proj.TryGetComponent<BaseProjectile>(out var bp))
                    bp.ProjectileDamageHandler.SetDamage(damage);

                proj.SetActive(true);
            }
        }
    }
}
