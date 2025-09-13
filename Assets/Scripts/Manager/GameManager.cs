using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneName
{
    Menu, 
    InGame
}

public class GameManager : Singleton<GameManager>
{
    private PlayerController playerInstance;
    bool isChanging = false;
    private int currentStageNum = -1;
    private GameObject currentStage = null;

    private GameObject[] stages;
    private GameObject playerPrefab;
    private const int STAGE_COUNT = 5;
    private CinemachineCamera cinemaCamera;

    protected override void Awake()
    {
        base.Awake();

        stages = new GameObject[STAGE_COUNT];
        for (int i = 0; i < STAGE_COUNT; i++)
        {
            var loadedStage = Resources.Load<GameObject>($"Stages/Stage_{i}");
            if (loadedStage != null)
            {
                stages[i] = loadedStage;
            }
        }
        var playerPrefab = Resources.Load<GameObject>("Player");
        if(playerPrefab != null)
        {
            this.playerPrefab = playerPrefab;
        }
    }

    private void Update()
    {
        if(currentStageNum != -1 )
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                ResetIngame();
            }
        }
    }

    public void LoadSelectMenu()
    {
        if (isChanging) return;

        StartCoroutine(LoadSelectMenuCoroutine());
    }

    public void LoadStage(int stage)
    {
        if (isChanging) return;

        StartCoroutine(LoadStageCoroutine(stage));
    }

    public void ResetCurrStage()
    {
        if (isChanging) return;
        StartCoroutine(ResetIngame());
    }

    private IEnumerator LoadSelectMenuCoroutine()
    {
        isChanging = true;
        currentStage = null;
        playerPrefab = null;
        StartCoroutine(UIManager.Instance.FadeOut());
        yield return new WaitForSeconds(UIManager.FADE_TIME);
        currentStageNum = -1;
        LoadScene(SceneName.Menu);
        StartCoroutine(UIManager.Instance.FadeIn());
        isChanging = false;
    }

    private IEnumerator LoadStageCoroutine(int stage)
    {
        isChanging = true;
        
        StartCoroutine(UIManager.Instance.FadeOut());
        yield return new WaitForSeconds(UIManager.FADE_TIME);
        if (currentStage != null)
        {
            Destroy(currentStage);
        }

        var asyncOp = LoadSceneAsync(SceneName.InGame);

        while (!asyncOp.isDone) yield return null;

        cinemaCamera = FindAnyObjectByType<CinemachineCamera>();
        playerInstance = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<PlayerController>();
        playerInstance.onDeath += ResetCurrStage;
        cinemaCamera.Follow = playerInstance.gameObject.transform;


        currentStageNum = stage;

        currentStage = Instantiate(stages[currentStageNum], Vector3.zero, Quaternion.identity);

        StartCoroutine(UIManager.Instance.FadeIn());
        ButtonManager.Instance.SetStageEvent();
        isChanging = false;
    }

    private AsyncOperation LoadSceneAsync(SceneName name)
    {
        if (SceneManager.GetActiveScene().name == name.ToString())
        {
            return null;
        }
        else
        {
            return SceneManager.LoadSceneAsync(name.ToString());
        }
    }

    private void LoadScene(SceneName name)
    {
        if(SceneManager.GetActiveScene().name == name.ToString())
        {
            return;
        }
        else
        {
            SceneManager.LoadScene(name.ToString());
        }
    }


    public IEnumerator ResetIngame()
    {
        isChanging = true;
        StartCoroutine(UIManager.Instance.FadeOut());
        yield return new WaitForSeconds(UIManager.FADE_TIME);

        ButtonManager.Instance.ResetStageEvent();
        
        Destroy(currentStage);
        Destroy(playerInstance.gameObject);

        yield return null;

        currentStage = Instantiate(stages[currentStageNum], Vector3.zero, Quaternion.identity);
        playerInstance = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<PlayerController>();

        playerInstance.onDeath += ResetCurrStage;
        cinemaCamera.Follow = playerInstance.gameObject.transform;


        ButtonManager.Instance.SetStageEvent();


        isChanging = false;
        StartCoroutine(UIManager.Instance.FadeIn());
    }

}