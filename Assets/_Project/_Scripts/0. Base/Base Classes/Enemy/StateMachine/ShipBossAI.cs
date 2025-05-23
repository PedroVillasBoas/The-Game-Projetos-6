using UnityEngine;
using GoodVillageGames.Game.Handlers;

namespace GoodVillageGames.Game.Core.Enemy.AI
{
    public class ShipBossAI : BossAI
    {
        [SerializeField] private RingFireHandler ringFireHandler;

        protected override void DoSpecialAction()
        {
            ringFireHandler.FireRing(transform.position, Stats.BaseAttackDamage);
        }
    }
}
