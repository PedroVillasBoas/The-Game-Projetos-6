using UnityEngine;


namespace GoodVillageGames.Game.General.UI.Animations
{
    public class TriggerLeaderboard : MonoBehaviour
    {
        [SerializeField] private PopulateLeaderboard leaderboard;

        public void SetLeaderboardTrigger()
        {
            leaderboard.LeaderboardTrigger();
        }
    }
}