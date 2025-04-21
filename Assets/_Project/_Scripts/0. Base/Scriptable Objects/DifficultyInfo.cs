using UnityEngine;
using TriInspector;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "DfficultyInfo", menuName = "Scriptable Objects/Info/Difficulty")]
    public class DifficultyInfo : ScriptableObject 
    { 
        public string Name;
        [TextArea] public string Description;
        public GameDifficulty gameDifficulty;
    }
}