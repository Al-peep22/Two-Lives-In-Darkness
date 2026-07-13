using UnityEngine;
using System.Collections;

public class UIFader : MonoBehaviour
{
    public void Fade(CanvasGroup group, bool fadeIn, float fadeDuration)
    {
        StartCoroutine(FadeRoutine(group, fadeIn ? 1f : 0f, fadeDuration));
    }

    private IEnumerator FadeRoutine(CanvasGroup group, float targetAlpha, float duration)
    {
        float startAlpha = group.alpha;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            group.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            yield return null;
        }

        group.alpha = targetAlpha;
    }
}
