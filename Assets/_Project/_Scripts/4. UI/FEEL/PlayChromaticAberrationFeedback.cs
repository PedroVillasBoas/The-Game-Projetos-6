namespace GoodVillageGames.Game.General.UI.Feel
{
    public class PlayChromaticAberrationFeedback : PlayFeedback
    {
        public override void Play() => TargetFeedback.PlayFeedbacks();
        public override void Stop() => TargetFeedback.StopFeedbacks();
    }
}