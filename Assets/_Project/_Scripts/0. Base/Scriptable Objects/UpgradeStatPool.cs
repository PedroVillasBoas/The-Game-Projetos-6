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
        [Group("Uncommon")] public List<UpgradeStatModifier> UncommonUpgrades;
        [Group("Rare")] public List<UpgradeStatModifier> RareUpgrades;
        [Group("Epic")] public List<UpgradeStatModifier> EpicUpgrades;
        [Group("Legendary")] public List<UpgradeStatModifier> LegendaryUpgrades;
    }
}