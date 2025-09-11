using DG.Tweening;
using UnityEngine;

enum UIType
{
    None,
    StageSelect,
    Settings,
    Exit
}

public class MenuUIManager : MonoBehaviour
{
    public float MoveXDistance = 1920f;
    public float MoveYDistance = 1080f;

    [SerializeField] private RectTransform TitleRectTrans;
    private Vector3 titleOriginPos;
    [SerializeField] private RectTransform StageSelectObj;
    private Vector3 stageSelectOriginPos;
    [SerializeField] private RectTransform SettingsRectTrans;
    private Vector3 settingsOriginPos;
    [SerializeField] private RectTransform ExitRectTrans;
    private Vector3 exitOriginPos;

    private float uiMoveTime = 0.5f;

    [SerializeField] private GameObject MenuSelectUI;

    UIType frontUI = UIType.None;

    private void Start()
    {
        GetOriginPos();
    }

    private void GetOriginPos()
    {
        titleOriginPos = TitleRectTrans.localPosition;
        stageSelectOriginPos = StageSelectObj.localPosition;
        settingsOriginPos = SettingsRectTrans.localPosition;
        exitOriginPos = ExitRectTrans.localPosition;
    }

    public void OnClickStageSelectButton()
    {
        if(frontUI != UIType.None) return;
        frontUI = UIType.StageSelect;
        TitleRectTrans.DOLocalMoveY(titleOriginPos.x - MoveXDistance, uiMoveTime).SetEase(Ease.InOutBack);
        StageSelectObj.DOLocalMoveY(stageSelectOriginPos.x - MoveXDistance, uiMoveTime).SetEase(Ease.InOutBack);
    }

    public void OnClickSettingsButton()
    {
        if (frontUI != UIType.None) return;
        frontUI = UIType.Settings;
        TitleRectTrans.DOLocalMoveX(titleOriginPos.y - MoveYDistance, uiMoveTime).SetEase(Ease.InOutBack);
        SettingsRectTrans.DOLocalMoveX(settingsOriginPos.y - MoveYDistance, uiMoveTime).SetEase(Ease.InOutBack);
    }

    public void OnClickExitButton()
    {
        if(frontUI != UIType.None) return;
        frontUI = UIType.Exit;
        ExitRectTrans.DOLocalMoveY(exitOriginPos.y - MoveYDistance, uiMoveTime).SetEase(Ease.InOutBack);
    }

    public void OnClickReturnButton()
    {
        if(frontUI == UIType.None) return;
        
        switch (frontUI)
        {
            case UIType.StageSelect:
                StageSelectObj.DOLocalMove(stageSelectOriginPos, uiMoveTime).SetEase(Ease.InOutBack);
                TitleRectTrans.DOLocalMove(titleOriginPos, uiMoveTime).SetEase(Ease.InOutBack);
                break;
            case UIType.Settings:
                SettingsRectTrans.DOLocalMove(settingsOriginPos, uiMoveTime).SetEase(Ease.InOutBack);
                TitleRectTrans.DOLocalMove(titleOriginPos, uiMoveTime).SetEase(Ease.InOutBack);
                break;
            case UIType.Exit:
                ExitRectTrans.DOLocalMove(exitOriginPos, uiMoveTime).SetEase(Ease.InOutBack);
                break;
        }

        frontUI = UIType.None;

    }

    public void OnClickQuitButton()
    {
        Application.Quit();
    }

}
