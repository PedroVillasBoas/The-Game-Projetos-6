using FMODUnity;
using FMOD.Studio;
using UnityEngine;
using GoodVillageGames.Game.Core.Global;
using GoodVillageGames.Game.Handlers.UI.Audio;

namespace GoodVillageGames.Game.Core
{
    public class PlayerAudioHandler : MonoBehaviour 
    { 
        public static PlayerAudioHandler Instance { get; private set; }

        private FMODEventsHandler fMODEventsHandler;
        private EventInstance moveEventInstance;

        void Awake() => fMODEventsHandler = FMODEventsHandler.Instance;

        void OnEnable() => InitializeMovementSFX();

        void InitializeMovementSFX()
        {
            moveEventInstance = RuntimeManager.CreateInstance(FMODEventsHandler.Instance.PlayerOnMove);
            moveEventInstance.start();
            moveEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }

        public void PlayPlayerMoveSFX()
        {
            moveEventInstance.setParameterByName("PlayerMove", 0); // OnMove Parameter on FMOD
        }

        public void PlayPlayerBoostSFX()
        {
            moveEventInstance.setParameterByName("PlayerMove", 1); // OnBoost Parameter on FMOD
        }

        public void PlayPlayerHitSFX()
        {
            GlobalAudioManager.Instance.PlayOneShotSound(fMODEventsHandler.PlayerOnHit, transform.position);
        }

        public void PlayPlayerDeathSFX()
        {
            GlobalAudioManager.Instance.PlayOneShotSound(fMODEventsHandler.PlayerOnDeath, transform.position);
        }

        public void PlayPlayerProjectileShootSFX()
        {
            GlobalAudioManager.Instance.PlayOneShotSound(fMODEventsHandler.PlayerOnProjectileOnShoot, transform.position);
        }

        public void PlayPlayerMissileShootSFX()
        {
            GlobalAudioManager.Instance.PlayOneShotSound(fMODEventsHandler.PlayerOnMissileShoot, transform.position);
        }

        public void PlayPlayerMissileOnCooldownShootSFX()
        {
            GlobalAudioManager.Instance.PlayOneShotSound(fMODEventsHandler.PlayerOnMissileShootInCooldown, transform.position);
        }
        public void PlayPlayerLevelUpSFX()
        {
            GlobalAudioManager.Instance.PlayOneShotSound(fMODEventsHandler.PlayerOnLevelUp, transform.position);
        }
    }
}
