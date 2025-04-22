using FMODUnity;
using UnityEngine;
using TriInspector;

namespace GoodVillageGames.Game.Handlers.UI.Audio
{
    [DeclareFoldoutGroup("UI"), DeclareFoldoutGroup("Music")]
    public class FMODEventsHandler : MonoBehaviour 
    { 
        public static FMODEventsHandler Instance { get; private set; }

        // SFX
        [field: SerializeField, Group("UI")] public EventReference ButtonEnter { get; private set; }
        [field: SerializeField, Group("UI")] public EventReference ButtonClick { get; private set; }

        // Music
        [field: SerializeField, Group("Music")] public EventReference GameMusic { get; private set; }

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
