using UnityEngine;
using TriInspector;
using GoodVillageGames.Game.Interfaces;
using GoodVillageGames.Game.Core.Util.Stats;

namespace GoodVillageGames.Game.General
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/Stats/PlayerStats")]
    public class PlayerStats : Stats
    {
        [Title("Passive")]
        public string PassiveName;
        public string PassiveDescription;
    }
}
