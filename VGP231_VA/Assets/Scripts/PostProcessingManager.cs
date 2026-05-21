using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingManager : MonoBehaviour
{
    private static PostProcessingManager _instance;
    public static PostProcessingManager Instance { get { return _instance; } }

    [Header("Post Processing")]

    private ColorAdjustments colorAdjustments;
    private Coroutine saturationCoroutine;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void LerpToSaturation(Volume postProcessVolume, float targetSaturation, float duration)
    {
        if (saturationCoroutine != null)
        {
            StopCoroutine(saturationCoroutine);
        }

        if (postProcessVolume.profile.TryGet(out colorAdjustments))
        {
            // Found Color Adjustments
        }

        saturationCoroutine = StartCoroutine(LerpSaturationCoroutine(targetSaturation, duration));
    }

    private IEnumerator LerpSaturationCoroutine(float targetSaturation, float duration)
    {
        float startSaturation = colorAdjustments.saturation.value;

        float timer = 0.0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;

            colorAdjustments.saturation.value = Mathf.Lerp(startSaturation, targetSaturation, t);

            yield return null;
        }

        colorAdjustments.saturation.value = targetSaturation;

        saturationCoroutine = null;
    }

    public float GetCurrentSaturation(Volume postProcessVolume)
    {
        postProcessVolume.profile.TryGet(out colorAdjustments);
        return colorAdjustments.saturation.value;
    }
}