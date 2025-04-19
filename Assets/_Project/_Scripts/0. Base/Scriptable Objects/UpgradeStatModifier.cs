using UnityEngine;
using TriInspector;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.Core.Attributes.Modifiers
{
    [CreateAssetMenu(fileName = "Upgrade", menuName = "Scriptable Objects/Upgrades/Upgrade")]
    public class UpgradeStatModifier : ScriptableObject 
    { 
        [Title("Upgrade")]
        public string Name;
        public string Description;
        public UpgradeRarity Rarity;
        public Sprite Image;
        [SerializeReference] public Upgrade UpgradeLogic;
    }
}