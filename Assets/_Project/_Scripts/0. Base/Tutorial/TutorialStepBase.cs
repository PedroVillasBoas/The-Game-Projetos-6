using TMPro;
using System;
using FMODUnity;
using UnityEngine;
using FMOD.Studio;
using TriInspector;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using GoodVillageGames.Game.Interfaces;
using UnityEngine.Localization.Components;

namespace GoodVillageGames.Game.General.Tutorial
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TutorialStepBase : MonoBehaviour, ITutorialStep, IPointerClickHandler
    {
        [Title("Typewriter Settings")]
        [Tooltip("Characters per second")]
        [SerializeField] private float typingSpeed = 30f;
        [Tooltip("Multiplier when player clicks while typing")]
        [SerializeField] private float fastSpeedMultiplier = 3f;

        [Title("UI References")]
        [SerializeField] private TMP_Text tutorialText;
        [SerializeField] private Button continueButton;

        [Title("Sound Reference")]
        [SerializeField] private EventReference eventReference;

        [Title("BlangBlong")]
        [SerializeField] private Animator blangblongAnimator;

        public event Action OnContinue;

        private EventInstance eventInstance;
        private CanvasGroup _canvasGroup;
        private float _currentSpeed;
        private string _fullText;
        private bool _isTyping;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            // Caching the full text and clear
            _fullText = tutorialText.text;
            tutorialText.text = string.Empty;

            // Hooking
            continueButton.onClick.AddListener(HandleContinue);
        }

        public void Enter()
        {
            // Show and enable interaction (Speed up, text!)
            gameObject.SetActive(true);
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;

            // Button disabled until text is done
            continueButton.gameObject.SetActive(false);

            // Start typing, Text!
            _currentSpeed = typingSpeed;
            StartCoroutine(InitializeAndPlay());

            // Start speaking, Text!
            eventInstance = RuntimeManager.CreateInstance(eventReference);
            eventInstance.start();
        }

        private IEnumerator InitializeAndPlay()
        {
            if (tutorialText.TryGetComponent<LocalizeStringEvent>(out var localizedStringEvent))
            {
                var asyncOperation = localizedStringEvent.StringReference.GetLocalizedStringAsync();
                yield return asyncOperation;

                _fullText = asyncOperation.Result;
            }
            else
            {
                _fullText = tutorialText.text;
                Debug.LogWarning("Nenhum componente LocalizeStringEvent encontrado no texto do tutorial. Usando texto padrão.");
            }

            tutorialText.text = string.Empty;
            StartCoroutine(TypeTextCoroutine());
        }

        private IEnumerator TypeTextCoroutine()
        {
            _isTyping = true;
            tutorialText.text = string.Empty;

            for (int i = 0; i <= _fullText.Length; i++)
            {
                tutorialText.text = _fullText[..i];
                yield return new WaitForSecondsRealtime(1f / _currentSpeed);
            }

            _isTyping = false;

            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();

            blangblongAnimator.SetTrigger("idle");
            continueButton.gameObject.SetActive(true);
        }

        public void Exit()
        {
            _canvasGroup.interactable = false;
            Destroy(gameObject);
        }

        private void HandleContinue() => OnContinue?.Invoke();

        // This fires when the player clicks anywhere on this UI element
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isTyping && eventData.button == PointerEventData.InputButton.Left)
            {
                _currentSpeed = typingSpeed * fastSpeedMultiplier;
                blangblongAnimator.SetFloat("talkSpeed", 2);
            }
        }
    }
}

