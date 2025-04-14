using UnityEngine;

namespace GoodVillageGames.Game.Core.Global
{
    /// <summary>
    /// This is a Global Class that controls the game and it's states;
    /// </summary>
    public class GlobalGameManager : MonoBehaviour
    {
        public static GlobalGameManager Instance { get; private set; }

        void Awake()
        {
            // Singleton
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }
    }
}
