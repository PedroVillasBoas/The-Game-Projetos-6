using FMODUnity;
using FMOD.Studio;
using UnityEngine;
using GoodVillageGames.Game.Handlers.UI.Audio;
using GoodVillageGames.Game.Core.Util;

namespace GoodVillageGames.Game.Core
{
    public class PlayerAudioHandler : MonoBehaviour 
    { 
        public static PlayerAudioHandler Instance { get; private set; }

        private IDTriggerOwner triggerOwner;
        private EventInstance moveEventInstance;

        void Awake()
        {
            // Singleton
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            triggerOwner = GetComponentInParent<IDTriggerOwner>();
        }

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
            triggerOwner.NotifySubscribers("player-hit");
        }

        public void PlayPlayerDeathSFX()
        {
            triggerOwner.NotifySubscribers("player-death");
        }

        public void PlayPlayerProjectileShootSFX()
        {
            triggerOwner.NotifySubscribers("player-base-shoot");
        }

        public void PlayPlayerMissileShootSFX()
        {
            triggerOwner.NotifySubscribers("player-missile-shoot");
        }

        public void PlayPlayerMissileOnCooldownShootSFX()
        {
            triggerOwner.NotifySubscribers("player-missile-shoot-oncooldown");
        }

        public void PlayPlayerLevelUpSFX()
        {
            triggerOwner.NotifySubscribers("player-levelup");
        }
    }
}
