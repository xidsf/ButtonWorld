using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Canvas fadeCanvas;
    [SerializeField] private Image fadeImage;
    public const float FADE_TIME = 0.5f;

    public IEnumerator FadeOut()
    {
        fadeImage.gameObject.SetActive(true);

        float elapsed = 0f;
        
        while (elapsed < FADE_TIME)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / FADE_TIME);
            SetFadeAlpha(alpha);
            yield return null;
        }

        SetFadeAlpha(1f);
    }

    public IEnumerator FadeIn()
    {
        fadeImage.gameObject.SetActive(true);
        float elapsed = 0f;

        while (elapsed < FADE_TIME)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(1f - (elapsed / FADE_TIME));
            SetFadeAlpha(alpha);
            yield return null;
        }

        SetFadeAlpha(0f);
        fadeImage.gameObject.SetActive(false);
    }

    private void SetFadeAlpha(float alpha)
    {
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = alpha;
            fadeImage.color = color;
        }
    }
}
