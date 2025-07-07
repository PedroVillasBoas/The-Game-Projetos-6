using GoodVillageGames.Game.Handlers;
using UnityEngine;

namespace GoodVillageGames.Game.General.UI.Feel
{
    public class PlayAllFeedbacks : PlayFeedback
    {
        [SerializeField] private HealthHandler player;

        void Start() => player.OnTookHit += Play;
        void OnDestroy() => player.OnTookHit -= Play;

        public override void Play() => TargetFeedback.PlayFeedbacks();
        public override void Stop() => TargetFeedback.StopFeedbacks();
    }
}