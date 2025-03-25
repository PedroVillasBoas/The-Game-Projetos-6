using UnityEngine;
using UnityEngine.SceneManagement;
using GoodVillageGames.Game.Core.ScriptableObjects;
using DG.Tweening;

namespace GoodVillageGames.Game.Core.Manager
{
    public class ChangeSceneManager : MonoBehaviour
    {
        // Singleton
        public ChangeSceneManager Instance { get; private set; }


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void ChangeScene(SceneScriptableObject _sceneSO)
        {
            if (_sceneSO != null)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneSO.Scene.ToString(), LoadSceneMode.Single);
                DOTween.KillAll();
            }
            else
                Debug.LogError($"Trying to load Scene: {_sceneSO.Scene}");
        }

        void SceneHasLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"Loaded scene: {scene.name} in mode: {mode}");
        }

        void OnEnable()
        {
            EventsManager.Instance.OnChangeSceneEventTriggered += ChangeScene;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneHasLoaded;
        }

        void OnDisable()
        {
            EventsManager.Instance.OnChangeSceneEventTriggered -= ChangeScene;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= SceneHasLoaded;
        }

    }
}
