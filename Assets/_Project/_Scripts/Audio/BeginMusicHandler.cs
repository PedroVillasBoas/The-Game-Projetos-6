using UnityEngine;
using GoodVillageGames.Game.Core.Global;

namespace GoodVillageGames.Game.Handlers.UI.Audio
{
    public class BeginMusicHandler : MonoBehaviour 
    {
        public void BeginMusic() => GlobalMusicManager.Instance.InitializeMusic();
    }
}