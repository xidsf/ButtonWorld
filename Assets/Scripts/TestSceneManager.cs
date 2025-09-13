using UnityEngine;

public class TestSceneManager : MonoBehaviour
{
    [SerializeField] private int testStageNumber;

    private void Start()
    {
        GameManager.Instance.LoadStage(testStageNumber);
    }
}
