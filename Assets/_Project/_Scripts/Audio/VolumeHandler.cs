using UnityEngine;
using UnityEngine.UI;
using GoodVillageGames.Game.Core.Global;
using static GoodVillageGames.Game.Enums.Enums;

namespace GoodVillageGames.Game.Handlers.UI.Audio
{
    public class VolumeHandler : MonoBehaviour 
    { 
        [SerializeField] private GameAudioBus bus;
        private Slider slider;

        void Awake() => slider = GetComponentInChildren<Slider>();
        void Update() => GetVolume();


        public void OnValueChanged()
        {
            switch (bus)
            {
                case GameAudioBus.Master:
                    GlobalAudioManager.Instance.masterVolume = slider.value;
                    break;
                case GameAudioBus.Music:
                    GlobalAudioManager.Instance.musicVolume = slider.value;
                    break;
                case GameAudioBus.SFX:
                    GlobalAudioManager.Instance.sfxVolume = slider.value;
                    break;
                case GameAudioBus.Ambient:
                    GlobalAudioManager.Instance.ambientVolume = slider.value;
                    break;
                default:
                    Debug.LogWarning($"Volume type on bus: {bus} not supported.");
                    break;
            }
        }

        void GetVolume()
        {
            switch (bus)
            {
                case GameAudioBus.Master:
                    slider.value = GlobalAudioManager.Instance.masterVolume;
                    break;
                case GameAudioBus.Music:
                    slider.value = GlobalAudioManager.Instance.musicVolume;
                    break;
                case GameAudioBus.SFX:
                    slider.value = GlobalAudioManager.Instance.sfxVolume;
                    break;
                case GameAudioBus.Ambient:
                    slider.value = GlobalAudioManager.Instance.ambientVolume;
                    break;
                default:
                    Debug.LogWarning($"Volume type on bus: {bus} not supported.");
                    break;
            }
        }
    }
}
