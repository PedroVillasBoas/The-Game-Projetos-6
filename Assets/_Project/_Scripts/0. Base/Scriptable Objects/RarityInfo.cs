using UnityEngine;
using GoodVillageGames.Game.Enums.Upgrades;

namespace GoodVillageGames.Game.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "RarityInfo", menuName = "Scriptable Objects/Info/Rarity")]
    public class RarityInfo : ScriptableObject 
    { 
        public string Name;
        [TextArea] public string Description;
        public UpgradeRarity Rarity;
    }
}