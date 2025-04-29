using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using GoodVillageGames.Game.Core.ScriptableObjects;

namespace GoodVillageGames.Game.Core.Manager
{
    public class ChangeSceneManager : MonoBehaviour
    {
        // Singleton
        public static ChangeSceneManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }

        public void ChangeScene(SceneScriptableObject targetScene)
        {
            if (targetScene != null)
                SceneManager.LoadScene(targetScene.Scene, LoadSceneMode.Single);
            else
                Debug.LogError("Target scene is not assigned in the inspector.");
        }
    }
}