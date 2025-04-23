using FMODUnity;
using GoodVillageGames.Game.Core.Util;
using UnityEngine;

namespace GoodVillageGames.Game.Handlers.Audio
{
    [RequireComponent(typeof(StudioEventEmitter))]
    public class IDTriggerFMODEvent : BaseIDTrigger
    {
        private StudioEventEmitter eventEmitter;
        
        protected override void Awake()
        {
            eventEmitter = GetComponent<StudioEventEmitter>();
            base.Awake();
        }

        public override void OnFired() => eventEmitter.Play();
    }
}