using UnityEngine;

namespace GoodVillageGames.Game.Core.Global
{
    public class GlobalDataColectorManager : MonoBehaviour
    {
        public static GlobalDataColectorManager Instance { get; private set; }

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
