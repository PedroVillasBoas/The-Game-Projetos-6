using GoodVillageGames.Game.Interfaces;
using UnityEngine;

namespace GoodVillageGames.Game.Core.Util
{
    public abstract class BaseIDTrigger : MonoBehaviour
    {
        public string id;
        public IDTriggerOwner ownerOverride;
        private IDTriggerOwner owner;

        protected virtual void Awake()
        {
            if (ownerOverride != null)
                owner = ownerOverride;
            else
            {
                var ownerProvider = GetComponentInParent<IDoppelganger<IDTriggerOwner>>();

                if (ownerProvider != null)
                    owner = ownerProvider.Owner;
                else
                    owner = GetComponentInParent<IDTriggerOwner>();
            }

            owner.SubscribeTrigger(id, this);
        }

        public void Fire()
        {
            if (isActiveAndEnabled)
                OnFired();
        }

        public abstract void OnFired();
    }


}