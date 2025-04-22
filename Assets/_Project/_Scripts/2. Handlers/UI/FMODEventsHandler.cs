using FMODUnity;
using UnityEngine;
using TriInspector;

namespace GoodVillageGames.Game.Handlers.UI.Audio
{
    [DeclareFoldoutGroup("UI")]
    public class FMODEventsHandler : MonoBehaviour 
    { 
        public static FMODEventsHandler Instance { get; private set; }

        [field: SerializeField, Group("UI")] public EventReference ButtonEnter { get; private set; }
        [field: SerializeField, Group("UI")] public EventReference ButtonClick { get; private set; }
        [field: SerializeField, Group("UI")] public EventReference ButtonExit { get; private set; }

        void Awake()
        {
            // Singleton
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }
    }
}
