using UnityEngine;
using GoodVillageGames.Game.Core.Attributes.Modifiers;

namespace GoodVillageGames.Game.Core.Manager
{
    public class PlayerUpgraderManager : MonoBehaviour
    {
        public static PlayerUpgraderManager Instance { get; private set; }

        [SerializeField] private PlayerActions player;

        void Awake()
        {
            // Singleton
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

        }

        public void AddUpgradeToPlayer(UpgradeStatModifier upgrade)
        {
            Debug.Log($"Upgrade: {upgrade}");
            upgrade.UpgradeLogic.ApplyUpgrade(player);
        }
    }
}