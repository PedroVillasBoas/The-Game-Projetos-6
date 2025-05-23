using System;
using FMODUnity;
using UnityEngine;
using FMOD.Studio;
using TriInspector;
using System.Collections.Generic;
using GoodVillageGames.Game.Enums;
using GoodVillageGames.Game.Enums.Audio;

namespace GoodVillageGames.Game.Core.Global
{
    public class GlobalAudioManager : MonoBehaviour
    {
        public static GlobalAudioManager Instance { get; private set; }

        [HideInInspector] public List<EventInstance> eventInstances;
        [HideInInspector] public List<StudioEventEmitter> eventEmitters;

        [Title("Volumes")]
        [Range(0, 1)] public float masterVolume = 1;
        [Range(0, 1)] public float musicVolume = 1;
        [Range(0, 1)] public float sfxVolume = 1;
        [Range(0, 1)] public float ambientVolume = 1;

        public EventInstance MusicEventInstance { get; set; }
        public EventInstance AmbientEventInstance { get; set; }

        // Audio Bus
        private Bus masterBus;
        private Bus musicBus;
        private Bus sfxBus;
        private Bus ambientBus;

        void Awake()
        {
            // Singleton
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);

            eventInstances = new();
            eventEmitters = new();

            masterBus = RuntimeManager.GetBus("bus:/");
            musicBus = RuntimeManager.GetBus("bus:/Music");
            sfxBus = RuntimeManager.GetBus("bus:/SFX");
            ambientBus = RuntimeManager.GetBus("bus:/Ambient");
        }


        void OnEnable() => GlobalEventsManager.Instance.ChangeGameStateEventTriggered += OnGameStateChanged;
        void OnDestroy() => GlobalEventsManager.Instance.ChangeGameStateEventTriggered -= OnGameStateChanged;

        private void OnGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.MainMenu:
                case GameState.Tutorial:
                case GameState.GameOver:
                    CleanUp();
                    break;
                default:
                    Debug.LogWarning($"Shouldn't change music in {state} State.\n Continuing...");
                    break;
            }
        }

        // For volume control in the UI
        public void SetMasterBusVolume(Bus bus, float value) => bus.setVolume(value);

        // For single sound event
        public void PlayOneShotSound(EventReference sound, Vector3 position)
        {
            RuntimeManager.PlayOneShot(sound, position);
        }

        // For multiple sound on same event (2D Timeline)
        public EventInstance CreateEventInstance(EventReference eventReference)
        {
            EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
            eventInstances.Add(eventInstance);
            return eventInstance;
        }

        // For Spacial Audio
        public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
        {
            StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
            emitter.EventReference = eventReference;
            eventEmitters.Add(emitter);
            return emitter;
        }

        public void SetVolume(GameAudioBus busType, float volume)
        {
            switch (busType)
            {
                case GameAudioBus.Master:
                    masterVolume = volume;
                    masterBus.setVolume(volume);
                    break;
                case GameAudioBus.Music:
                    musicVolume = volume;
                    musicBus.setVolume(volume);
                    break;
                case GameAudioBus.SFX:
                    sfxVolume = volume;
                    sfxBus.setVolume(volume);
                    break;
                case GameAudioBus.Ambient:
                    ambientVolume = volume;
                    ambientBus.setVolume(volume);
                    break;
                default:
                    Debug.LogWarning($"Bus type {busType} not supported.");
                    break;
            }
        }

        // When chaging Scenes
        void CleanUp()
        {
            foreach (EventInstance instance in eventInstances)
            {
                instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                instance.release();
            }

            foreach (StudioEventEmitter instance in eventEmitters)
            {
                instance.Stop();
            }
        }
    }
}
