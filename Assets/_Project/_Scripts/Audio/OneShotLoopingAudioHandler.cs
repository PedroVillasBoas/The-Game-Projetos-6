using UnityEngine;
using FMOD.Studio;
using TriInspector;
using FMODUnity;

namespace GoodVillageGames.Game.Handlers.UI.Audio
{
    public class OneShotLoopingAudioHandler : OneShotAudioHandler 
    {
        [Title("Disable Flags")]
        [SerializeField] private bool disableOnDisable = false;
        [SerializeField] private bool disableOnDestroy = false;

        [Title("Stop Mode Flag")]
        [SerializeField] private bool allowFading = false;

        protected override void OnDisable()
        {
            if (disableOnDisable) DisableAudio();
        }

        protected override void OnDestroy()
        {
            if (disableOnDestroy) DisableAudio();
        }

        protected override void Play()
        {
            eventInstance = RuntimeManager.CreateInstance(eventReference);
            eventInstance.start();
        }

        public virtual void DisableAudio()
        {
            eventInstance.stop(allowFading ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT : FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }
}