using FMODUnity;
using UnityEngine;
using TriInspector;
using FMOD.Studio;

namespace GoodVillageGames.Game.Handlers.UI.Audio
{
    public class OneShotAudioHandler : MonoBehaviour 
    {
        [Title("Event")]
        [SerializeField] protected EventReference eventReference;

        [Title("Play Flags")]
        [SerializeField] protected bool playOnStart = false;
        [SerializeField] protected bool playOnEnable = false;
        [SerializeField] protected bool playOnDisable = false;
        [SerializeField] protected bool playOnDestroy = false;

        protected EventInstance eventInstance;

        protected virtual void OnEnable()
        {
            if (playOnEnable) Play();
        }

        protected virtual void Start()
        {
            if (playOnStart) Play();
        }

        protected virtual void OnDisable()
        {
            if (playOnDisable) Play();
        }

        protected virtual void OnDestroy()
        {
            if (playOnDestroy) Play();
        }

        protected virtual void Play()
        {
            eventInstance = RuntimeManager.CreateInstance(eventReference);
            eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
            eventInstance.start();
        }

        protected virtual void StopOneShot() 
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }
}