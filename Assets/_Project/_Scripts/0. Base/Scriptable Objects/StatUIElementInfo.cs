using UnityEngine;

namespace GoodVillageGames.Game.Core.Attributes
{
    [CreateAssetMenu(fileName = "StatUIElement", menuName = "Scriptable Objects/StatUIElementInfo/Stat")]
    public class StatUIElementInfo : ScriptableObject
    {
        public string Name;
        public string Description;
        public string PortName;
        public string PortDescription;
    }

}