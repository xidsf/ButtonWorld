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
    public const int STAGE_COUNT = 5;

    private PlayerController playerInstance;
    bool isChanging = false;
    private int currentStageNum = -1;
    private GameObject currentStage = null;

    public int CurrentStageNum { get { return currentStageNum; } }
    public void SetStageToNext()
    {
        currentStageNum++;
        if (currentStageNum >= STAGE_COUNT)
        {
            currentStageNum = STAGE_COUNT - 1;
        }
    }

    private GameObject[] stages;
    private GameObject playerPrefab;
    private CinemachineCamera cinemaCamera;

    // 리셋 쿨타임 관련 변수 추가
    private float lastResetTime = -10f;
    private const float resetCooldown = 2f; 
    public bool IsStageResetable { get { return(Time.time - lastResetTime >= resetCooldown); } }

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
        ButtonController.ResetAudioPlay();
        lastResetTime = Time.time;
    }

    public void ResetCurrStage(bool isNextStage = false)
    {
        if (isChanging) return;
        if (Time.time - lastResetTime < resetCooldown && playerInstance.StateType != PlayerStateType.Death) return;
        StartCoroutine(ResetIngame(isNextStage));
        ButtonController.ResetAudioPlay();
        lastResetTime = Time.time;
    }

    private IEnumerator LoadSelectMenuCoroutine()
    {
        isChanging = true;
        AudioManager.Instance.StopBGM();
        currentStage = null;
        playerInstance = null;
        StartCoroutine(UIManager.Instance.FadeOut());
        yield return new WaitForSeconds(UIManager.FADE_TIME);
        UIManager.Instance.HideAllUI();
        currentStageNum = -1;
        AudioManager.Instance.PlayBGM(BGM.Title);
        LoadScene(SceneName.Menu);

        isChanging = false;
    }

    private IEnumerator LoadStageCoroutine(int stage)
    {
        isChanging = true;
        AudioManager.Instance.StopBGM();
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
        //playerInstance.onDeath += ResetCurrStage;
        cinemaCamera.Follow = playerInstance.gameObject.transform;

        currentStageNum = stage;

        currentStage = Instantiate(stages[currentStageNum], Vector3.zero, Quaternion.identity);

        StartCoroutine(UIManager.Instance.FadeIn());
        ButtonManager.Instance.SetStageEvent();
        AudioManager.Instance.PlayBGM(BGM.InGame);

        isChanging = false;
    }

    public IEnumerator ResetIngame(bool isNextStage = false)
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

        //playerInstance.onDeath += ResetCurrStage;
        cinemaCamera.Follow = playerInstance.gameObject.transform;

        ButtonManager.Instance.SetStageEvent();

        UIManager.Instance.HideAllUI();

        StartCoroutine(UIManager.Instance.FadeIn());
        AudioManager.Instance.PlayBGM(BGM.InGame);
        if (!isNextStage)
        {
            AudioManager.Instance.PlaySFX(SFX.GameOver);
        }
        
        yield return new WaitForSeconds(UIManager.FADE_TIME);

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
        if (SceneManager.GetActiveScene().name == name.ToString())
        {
            return;
        }
        else
        {
            SceneManager.LoadScene(name.ToString());
        }
    }


    public void ClearStage()
    {
        if (isChanging) return;
        StartCoroutine(ClearCoroutine());
    }

    IEnumerator ClearCoroutine()
    {
        isChanging = true;
        AudioManager.Instance.StopBGM();
        yield return new WaitForSeconds(1.3f);
        AudioManager.Instance.PlaySFX(SFX.Victory);
        UIManager.Instance.EnableClearUI();
        isChanging = false;
    }
}