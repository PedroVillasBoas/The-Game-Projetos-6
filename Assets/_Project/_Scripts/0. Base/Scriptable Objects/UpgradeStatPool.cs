using UnityEngine;
using TriInspector;
using System.Collections.Generic;

namespace GoodVillageGames.Game.Core.Attributes.Modifiers
{
    [CreateAssetMenu(fileName = "UpgradePool", menuName = "Scriptable Objects/Upgrades/Upgrades Pool")]
    [DeclareBoxGroup("Common", HideTitle = false, Title = "Common")]
    [DeclareBoxGroup("Uncommon", HideTitle = false, Title = "Uncommon")]
    [DeclareBoxGroup("Rare", HideTitle = false, Title = "Rare")]
    [DeclareBoxGroup("Epic", HideTitle = false, Title = "Epic")]
    [DeclareBoxGroup("Legendary", HideTitle = false, Title = "Legendary")]
    public class UpgradeStatPool : ScriptableObject 
    { 
        [Group("Common")] public List<UpgradeStatModifier> CommonUpgrades;
        [Group("Common")] public float CommonWeight;
        [Group("Uncommon")] public List<UpgradeStatModifier> UncommonUpgrades;
        [Group("Uncommon")] public float UncommonWeight;
        [Group("Rare")] public List<UpgradeStatModifier> RareUpgrades;
        [Group("Rare")] public float RareWeight;
        [Group("Epic")] public List<UpgradeStatModifier> EpicUpgrades;
        [Group("Epic")] public float EpicWeight;
        [Group("Legendary")] public List<UpgradeStatModifier> LegendaryUpgrades;
        [Group("Legendary")] public float LegendaryWeight;
    }
}