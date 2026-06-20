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
    private ColorLookup colorLookup;

    private Coroutine saturationCoroutine;
    private Coroutine colorLookupCoroutine;

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

    #region Color Lookup

    public void LerpToColorLookupContribution(
        Volume postProcessVolume,
        float targetContribution,
        float duration)
    {
        if (colorLookupCoroutine != null)
        {
            StopCoroutine(colorLookupCoroutine);
        }

        if (postProcessVolume.profile.TryGet(out colorLookup))
        {
            colorLookupCoroutine = StartCoroutine(
                LerpColorLookupContributionCoroutine(targetContribution, duration));
        }
    }

    private IEnumerator LerpColorLookupContributionCoroutine(
        float targetContribution,
        float duration)
    {
        float startContribution = colorLookup.contribution.value;

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;

            colorLookup.contribution.value =
                Mathf.Lerp(startContribution, targetContribution, t);

            yield return null;
        }

        colorLookup.contribution.value = targetContribution;

        colorLookupCoroutine = null;
    }

    public float GetCurrentColorLookupContribution(Volume postProcessVolume)
    {
        if (postProcessVolume.profile.TryGet(out colorLookup))
        {
            return colorLookup.contribution.value;
        }

        return 0f;
    }

    public void SetColorLookup(Volume postProcessVolume, Texture lookupTexture)
    {
        if (postProcessVolume.profile.TryGet(out colorLookup))
        {
            colorLookup.texture.value = lookupTexture;
        }
    }

    #endregion
}