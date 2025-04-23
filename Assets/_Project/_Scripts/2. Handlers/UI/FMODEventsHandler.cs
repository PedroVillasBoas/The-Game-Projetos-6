using FMODUnity;
using UnityEngine;
using TriInspector;
using System.Reflection;
using System.Collections.Generic;

namespace GoodVillageGames.Game.Handlers.UI.Audio
{
    [DeclareFoldoutGroup("UI"), DeclareFoldoutGroup("Music")]
    public class FMODEventsHandler : MonoBehaviour 
    { 
        public static FMODEventsHandler Instance { get; private set; }
        public List<string> EventNames { get; private set; }

        // SFX
            // Geral
        [field: SerializeField, Group("UI"), Indent] public EventReference ButtonEnter { get; private set; }
        [field: SerializeField, Group("UI"), Indent(2)] public EventReference ButtonClick { get; private set; }
            // Boot Splash
        [field: SerializeField, Group("UI"), Indent] public EventReference BootSplashEnd { get; private set; }
            // Splash Screen
        [field: SerializeField, Group("UI"), Indent] public EventReference SplashScreenTransition { get; private set; }

        // Music
        [field: SerializeField, Group("Music")] public EventReference GameMusic { get; private set; }

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }
    }
}