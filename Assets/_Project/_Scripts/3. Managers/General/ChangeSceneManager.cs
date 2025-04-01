using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement;
using GoodVillageGames.Game.Core.ScriptableObjects;

namespace GoodVillageGames.Game.Core.Manager
{
    public class ChangeSceneManager : MonoBehaviour
    {
        // Singleton
        public static ChangeSceneManager Instance { get; private set; }

        // Loading Scene
        [SerializeField] SceneScriptableObject _loadingScene;

        private SceneScriptableObject _sceneToLoad;
        private string _lastSceneLoaded;

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
                _lastSceneLoaded = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;;
                _sceneToLoad = _sceneSO;
                DOTween.KillAll();
                UnityEngine.SceneManagement.SceneManager.LoadScene(_loadingScene.Scene, LoadSceneMode.Single);
            }
            else
                Debug.LogError($"Trying to load Scene: {_sceneSO.Scene}");
        }

        public SceneScriptableObject GetPendingScene()
        {
            return _sceneToLoad;
        }

        public string GetLastLoadedScene()
        {
            return _lastSceneLoaded;
        }

        void OnEnable()
        {
            EventsManager.Instance.OnChangeSceneEventTriggered += ChangeScene;
        }

        void OnDisable()
        {
            EventsManager.Instance.OnChangeSceneEventTriggered -= ChangeScene;
        }

    }
}
