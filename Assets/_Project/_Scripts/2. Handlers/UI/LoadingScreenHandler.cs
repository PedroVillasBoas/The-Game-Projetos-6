using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using GoodVillageGames.Game.Core.Manager;
using GoodVillageGames.Game.Core.ScriptableObjects;
using UnityEngine.UI;

namespace GoodVillageGames.Game.Handlers.UI
{
    public class LoadingScreenHandler : MonoBehaviour
    {
        // Loading Bar
        [SerializeField] private GameObject loadingbarUI;
        [SerializeField] private Image loadingbarFill;
        // Prompt
        [SerializeField] private GameObject promptUI;

        private void Start()
        {
            promptUI.SetActive(false);
            loadingbarFill.fillAmount = 0;
            StartCoroutine(LoadTargetScene());
        }

        IEnumerator LoadTargetScene()
        {
            SceneScriptableObject sceneToLoad = ChangeSceneManager.Instance.GetPendingScene();

            AsyncOperation op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneToLoad.Scene, LoadSceneMode.Additive);
            op.allowSceneActivation = false;

            while (op.progress < 0.9f)
            {
                loadingbarFill.fillAmount = op.progress / 0.9f;
                yield return null;
            }

            op.allowSceneActivation = true;
            yield return new WaitUntil(() => op.isDone);

            Scene targetScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneToLoad.Scene);

            if (!targetScene.IsValid()) 
            {
                Debug.LogError("Scene is not valid, please check if it has been loaded correctly.");
                yield break;
            }

            if (op.isDone)
            {
                loadingbarUI.SetActive(false);
                promptUI.SetActive(true);
            }
            else
                Debug.LogError("Async Operation did something wrong!");
        }

        public void OnPromptClicked()
        {
            StartCoroutine(UnloadLoadingScene());
        }

        IEnumerator UnloadLoadingScene()
        {
            SceneScriptableObject targetScene = ChangeSceneManager.Instance.GetPendingScene();
            UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByName(targetScene.Scene));
            yield return UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Scene-Loading");
        }
    }
}
