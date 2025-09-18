using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Canvas fadeCanvas;
    [SerializeField] private Image fadeImage;
    [SerializeField] private GameObject ClearUI;
    [SerializeField] private GameObject nextStageButton;   
    [SerializeField] private GameObject PauseUI;

    public const float FADE_TIME = 0.5f;

    public bool IsClear { get { return isClear; } }
    private bool isClear = false;
    public bool IsOpenIngameMenu { get { return isOpenIngameMenu; } }
    private bool isOpenIngameMenu = false;

    private bool isFadeing = false;

    public IEnumerator FadeOut()
    {
        fadeImage.gameObject.SetActive(true);
        isFadeing = true;
        float elapsed = 0f;
        
        while (elapsed < FADE_TIME)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / FADE_TIME);
            SetFadeAlpha(alpha);
            yield return null;
        }
        isFadeing = false;
        SetFadeAlpha(1f);
    }

    public IEnumerator FadeIn()
    {
        fadeImage.gameObject.SetActive(true);
        isFadeing = true;
        float elapsed = 0f;

        while (elapsed < FADE_TIME)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(1f - (elapsed / FADE_TIME));
            SetFadeAlpha(alpha);
            yield return null;
        }

        SetFadeAlpha(0f);
        isFadeing = false;
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

    public void EnableClearUI()
    {
        if(GameManager.Instance.CurrentStageNum == -1) return;
        isClear = true;
        DisableIngameMenuUI();
        if (GameManager.Instance.CurrentStageNum == GameManager.STAGE_COUNT - 1)
            nextStageButton.SetActive(false);
        else
            nextStageButton.SetActive(true);
        ClearUI.SetActive(true);
    }

    public void OnClickReturnMenuButton()
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonPress);
        GameManager.Instance.LoadSelectMenu();
        Invoke("DisableClearUI", FADE_TIME);
        DisableIngameMenuUI();
    }

    public void OnClickNextStageButton()
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonPress);
        GameManager.Instance.SetStageToNext();
        GameManager.Instance.ResetCurrStage(true);
    }

    private void DisableClearUI()
    {
        isClear = false;
        ClearUI.SetActive(false);
    }

    public void EnableIngameMenuUI()
    {
        if (GameManager.Instance.CurrentStageNum == -1) return;
        if (isClear || isFadeing) return;
        isOpenIngameMenu = true;
        PauseUI.SetActive(true);
    }

    public void DisableIngameMenuUI()
    {
        isOpenIngameMenu = false;
        PauseUI.SetActive(false);
    }

    public void HideAllUI()
    {
        DisableClearUI();
        DisableIngameMenuUI();
    }

}
