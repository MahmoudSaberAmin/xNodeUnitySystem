using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Custom Fade Animation instead of the Tween fade animation which does not work with images
/// </summary>
public class FadeTween : MonoBehaviour
{
    public void FadeTo(Image image, float time, float endAlpha)
    {
        StartCoroutine(FadeCoroutine(image, time, endAlpha));
    }

    private IEnumerator FadeCoroutine(Image image, float time, float endAlpha)
    {
        var lerpAmount = 0f;
        var startAlpha = image.color.a;
        while (lerpAmount < 1)
        {
            SetAlpha(image, startAlpha, endAlpha, lerpAmount);
            lerpAmount += Time.deltaTime/time;
            yield return null;
        }

        SetAlpha(image, startAlpha, endAlpha, 1);

        Destroy(this);
    }

    private void SetAlpha(Image image, float startAlpha,float endAlpha, float lerpAmount)
    {
        Color c = image.color;
        c.a = Mathf.Lerp(startAlpha, endAlpha, lerpAmount);
        image.color = c;
    }
}
