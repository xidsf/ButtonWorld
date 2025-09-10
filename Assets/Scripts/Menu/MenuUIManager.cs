using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    bool isClickButton = false;

    public float MoveDistance = 500f;
    [SerializeField] private GameObject[] MoveUIObjects;
    private RectTransform[] MoveUIObjectOriginPos;

    [SerializeField] private GameObject MenuSelectUI;

    private void Start()
    {
        GetOriginPos();
    }

    private void GetOriginPos()
    {
        var objCount = MoveUIObjects.Length;

        MoveUIObjectOriginPos = new RectTransform[objCount];

        for (int i = 0; i < objCount; i++)
        {
            MoveUIObjectOriginPos[i] = MoveUIObjects[i].GetComponent<RectTransform>();
        }
    }

    public void OnClickStageSelectButton()
    {

    }

    public void OnClickReturnToTitleButton()
    {

    }

    public void OnClickSettingsButton()
    {

    }

    public void OnClickExitButton()
    {
        
    }
}
