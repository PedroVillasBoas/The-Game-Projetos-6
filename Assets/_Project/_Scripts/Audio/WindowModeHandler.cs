using UnityEngine;
using UnityEngine.UI;

namespace GoodVillageGames.Game.Handlers.UI.Options
{
    public class WindowModeHandler : MonoBehaviour 
    { 
        [SerializeField] private Toggle fullscreenToggle;
        [SerializeField] private Toggle windowedToggle;
        
        private bool _isSwitching;

        private void Awake()
        {
            if (fullscreenToggle == null || windowedToggle == null)
            {
                Debug.LogError($"Toggles not assigned in {gameObject.name}!");
                return;
            }
        }

        private void Start()
        {
            InitializeToggles();
            fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggleChanged);
            windowedToggle.onValueChanged.AddListener(OnWindowedToggleChanged);
        }

        private void InitializeToggles()
        {
            _isSwitching = true;
            
            // Setting initial display state based on current screen mode
            bool isFullscreen = Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen;
            fullscreenToggle.isOn = isFullscreen;
            windowedToggle.isOn = !isFullscreen;
            
            _isSwitching = false;
        }

        private void OnFullscreenToggleChanged(bool isOn)
        {
            if (_isSwitching) return;
            
            _isSwitching = true;
            
            if (isOn)
            {
                SetFullscreen();
                windowedToggle.isOn = false;
            }
            
            _isSwitching = false;
        }

        private void OnWindowedToggleChanged(bool isOn)
        {
            if (_isSwitching) return;
            
            _isSwitching = true;
            
            if (isOn)
            {
                SetWindowed();
                fullscreenToggle.isOn = false;
            }
            
            _isSwitching = false;
        }

        private void SetFullscreen()
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            // Optional: Set resolution to native display resolution
            Resolution nativeResolution = Screen.currentResolution;
            Screen.SetResolution(nativeResolution.width, nativeResolution.height, FullScreenMode.ExclusiveFullScreen);
        }

        private void SetWindowed()
        {
            //Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
            
            // Optional: Keep current resolution in windowed mode
            Resolution currentRes = Screen.currentResolution;
            Screen.SetResolution(currentRes.width, currentRes.height, FullScreenMode.Windowed);
        }
    }
}