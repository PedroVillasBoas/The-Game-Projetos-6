using FMODUnity;
using UnityEngine;


namespace GoodVillageGames.Game.Core.Global
{
    public class GlobalAudioManager : MonoBehaviour
    {
        public static GlobalAudioManager Instance { get; private set; }

        void Awake()
        {
            // Singleton
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        public void PlayerOneShotSound(EventReference sound, Vector3 position)
        {
            RuntimeManager.PlayOneShot(sound, position);
        }
    }
}
