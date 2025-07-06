using UnityEngine;
using MoreMountains.Feedbacks;

namespace GoodVillageGames.Game.General.UI.Feel
{
    public abstract class PlayFeedback : MonoBehaviour
    {
        [SerializeField] protected MMF_Player TargetFeedback;

        public abstract void Play();
        public abstract void Stop();
    }
}