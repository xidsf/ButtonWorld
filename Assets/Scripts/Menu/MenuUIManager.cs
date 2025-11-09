using DG.Tweening;
using System.Collections;
using UnityEngine;

enum UIType
{
    None,
    StageSelect
}

public class MenuUIManager : MonoBehaviour
{
    public float MoveXDistance = 1920f;
    public float MoveYDistance = 1080f;

    [SerializeField] private RectTransform MainUITrans;
    private Vector3 mainUITransOriginPos;

    private float uiMoveTime = 0.5f;
    private bool isChanging = false;

    UIType frontUI = UIType.None;

    private void Start()
    {
        StartCoroutine(UIManager.Instance.FadeIn());
        SetOriginPos();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickReturnButton();
        }
    }

    private void SetOriginPos()
    {
        mainUITransOriginPos = MainUITrans.localPosition;
    }

    public void OnClickStageSelectButton()
    {
        if(frontUI != UIType.None || isChanging) return;
        StartCoroutine(ConversionTimeCoroutine());
        AudioManager.Instance.PlaySFX(SFX.ButtonPress);
        MainUITrans.DOLocalMoveY(mainUITransOriginPos.y + MoveYDistance, uiMoveTime).SetEase(Ease.InOutBack);

        frontUI = UIType.StageSelect;
    }

    public void OnClickExitButton()
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonPress);
        Application.Quit();
    }

    public void OnClickReturnButton()
    {
        if(frontUI != UIType.StageSelect || isChanging) return;
        StartCoroutine(ConversionTimeCoroutine());
        AudioManager.Instance.PlaySFX(SFX.ButtonPress);
        MainUITrans.DOLocalMoveY(mainUITransOriginPos.y, uiMoveTime).SetEase(Ease.InOutBack);
        frontUI = UIType.None;
    }

    IEnumerator ConversionTimeCoroutine()
    {
        if(isChanging) yield break;
        isChanging = true;
        yield return new WaitForSeconds(uiMoveTime);
        isChanging = false;
    }

    public void OnClickStageButton(int stageNum)
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonPress);
        isChanging = true;
        GameManager.Instance.LoadStage(stageNum);
    }

}
