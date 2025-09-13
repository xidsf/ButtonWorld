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

    [SerializeField] private RectTransform TitleRectTrans;
    private Vector3 titleOriginPos;
    [SerializeField] private RectTransform StageSelectObj;
    private Vector3 stageSelectOriginPos;

    private float uiMoveTime = 0.5f;
    private bool isChanging = false;

    UIType frontUI = UIType.None;

    private void Start()
    {
        StartCoroutine(UIManager.Instance.FadeIn());
        GetOriginPos();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickReturnButton();
        }
    }

    private void GetOriginPos()
    {
        titleOriginPos = TitleRectTrans.localPosition;
        stageSelectOriginPos = StageSelectObj.localPosition;
    }

    public void OnClickStageSelectButton()
    {
        if(frontUI != UIType.None || isChanging) return;
        StartCoroutine(ConversionTimeCoroutine());

        TitleRectTrans.DOLocalMoveX(titleOriginPos.x - MoveXDistance, uiMoveTime).SetEase(Ease.InOutBack);
        StageSelectObj.DOLocalMoveX(stageSelectOriginPos.x - MoveXDistance, uiMoveTime).SetEase(Ease.InOutBack);

        frontUI = UIType.StageSelect;
    }

    public void OnClickExitButton()
    {
        Application.Quit();
    }

    public void OnClickReturnButton()
    {
        if(frontUI != UIType.StageSelect || isChanging) return;
        StartCoroutine(ConversionTimeCoroutine());

        TitleRectTrans.DOLocalMoveX(titleOriginPos.x, uiMoveTime).SetEase(Ease.InOutBack);
        StageSelectObj.DOLocalMoveX(stageSelectOriginPos.x, uiMoveTime).SetEase(Ease.InOutBack);

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
        isChanging = true;
        GameManager.Instance.LoadStage(stageNum);
    }

}
