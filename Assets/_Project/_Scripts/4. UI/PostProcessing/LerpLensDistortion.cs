using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LerpLensDistortion : MonoBehaviour
{
    [Header("Target Volume")]
    [Tooltip("The Volume component that contains the Lens Distortion effect.")]
    [SerializeField] private Volume volume;

    [Header("Lerp Configuration")]
    [Tooltip("The starting intensity value for the lens distortion.")]
    [SerializeField] private float startIntensity = 0f;

    [Tooltip("The target intensity value for the lens distortion.")]
    [SerializeField] private float endIntensity = 0.7f;

    [Tooltip("The time in seconds it takes to transition from start to end intensity.")]
    [SerializeField] private float lerpDuration = 1.5f;

    [Header("Behavior")]
    [Tooltip("If checked, the effect will automatically reverse and fade out after reaching the end intensity.")]
    [SerializeField] private bool autoReverse = false;

    [Tooltip("If checked, the effect will loop continuously. Requires Auto Reverse for a smooth loop.")]
    [SerializeField] private bool shouldLoop = false;

    private Coroutine _lerpCoroutine;
    private LensDistortion _lensDistortion;

    /// <summary>
    /// Public method to be called by a UnityEvent (e.g., a Button's OnClick event).
    /// This starts the process of lerping the lens distortion effect.
    /// </summary>
    public void TriggerLerp()
    {
        if (_lensDistortion == null)
        {
            if (volume == null || !volume.profile.TryGet(out _lensDistortion))
            {
                Debug.LogError("LerpLensDistortion: Volume is not assigned or does not have a Lens Distortion override.");
                return;
            }
        }

        // This allows the effect to be re-triggered smoothly
        if (_lerpCoroutine != null)
            StopCoroutine(_lerpCoroutine);

        // Start the main coroutine and store a reference to it
        _lerpCoroutine = StartCoroutine(LerpEffect());
    }

    /// <summary>
    /// Public method to stop the effect from looping.
    /// The current cycle will complete, and then the coroutine will stop.
    /// </summary>
    public void StopLooping() => shouldLoop = false;
    
    /// <summary>
    /// The coroutine that handles the value transition over time.
    /// </summary>
    private IEnumerator LerpEffect()
    {
        // A do-while loop ensures the effect runs at least once, and then repeats if shouldLoop is true
        do
        {
            float elapsedTime = 0f;

            // --- FORWARD LERP (Start to End) ---
            while (elapsedTime < lerpDuration)
            {
                elapsedTime += Time.deltaTime;

                float t = Mathf.Clamp01(elapsedTime / lerpDuration);

                // Using SmoothStep for a more aesthetically pleasing ease-in and ease-out effect
                float smoothedT = Mathf.SmoothStep(0f, 1f, t);

                // The .value property is what we need to change on the URP parameter.
                _lensDistortion.intensity.value = Mathf.Lerp(startIntensity, endIntensity, smoothedT);

                yield return null;
            }

            _lensDistortion.intensity.value = endIntensity;

            // --- AUTO-REVERSE LERP (End to Start) ---
            if (autoReverse)
            {
                elapsedTime = 0f;

                while (elapsedTime < lerpDuration)
                {
                    elapsedTime += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsedTime / lerpDuration);
                    float smoothedT = Mathf.SmoothStep(0f, 1f, t);

                    // Lerp from the end value back to the start value
                    _lensDistortion.intensity.value = Mathf.Lerp(endIntensity, startIntensity, smoothedT);

                    yield return null;
                }

                // Ensure the value is exactly back at the start intensity
                _lensDistortion.intensity.value = startIntensity;
            }
        } while (shouldLoop); // If shouldLoop is true, the entire process will repeat

        // The coroutine has finished, so we clear the reference
        _lerpCoroutine = null;
    }
}