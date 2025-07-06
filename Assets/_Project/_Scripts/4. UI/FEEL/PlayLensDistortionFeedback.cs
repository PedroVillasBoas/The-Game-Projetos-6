using MoreMountains.FeedbacksForThirdParty;

namespace GoodVillageGames.Game.General.UI.Feel
{
    public class PlayLensDistortionFeedback : PlayFeedback
    {
        private MMF_LensDistortion_URP _lensDistortion;

        void Start() => _lensDistortion = TargetFeedback.GetFeedbackOfType<MMF_LensDistortion_URP>();

        public override void Play() => _lensDistortion.Active = true;
        public override void Stop() => _lensDistortion.Active = false;
    }
}