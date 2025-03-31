using UnityEngine;
using TriInspector;
using UnityEngine.SceneManagement;

namespace GoodVillageGames.Game.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SceneSO", menuName = "Scriptable Objects/Scene")]
    public class SceneScriptableObject : ScriptableObject
    {
        [Scene]
        public string Scene;
    }
}
