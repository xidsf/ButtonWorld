using System.Collections;
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
    private PlayerController player;
    bool isChanging = false;
    private int currentStageNum = -1;
    private GameObject currentStage = null;

    public void LoadSelectMenu()
    {
        if (isChanging)
        {
            return;
        }

        StartCoroutine(LoadSelectMenuCoroutine());
    }

    public void LoadStage(int stage)
    {
        if (isChanging)
        {
            return;
        }
        StartCoroutine(LoadStageCoroutine(stage));
    }

    private IEnumerator LoadSelectMenuCoroutine()
    {
        
        isChanging = true;
        currentStage = null;
        StartCoroutine(UIManager.Instance.FadeIn());
        yield return new WaitForSeconds(UIManager.FADE_TIME);
        currentStageNum = -1;
        LoadScene(SceneName.Menu);
        StartCoroutine(UIManager.Instance.FadeOut());
        isChanging = false;
    }

    private IEnumerator LoadStageCoroutine(int stage)
    {
        isChanging = true;
        
        StartCoroutine(UIManager.Instance.FadeIn());
        yield return new WaitForSeconds(UIManager.FADE_TIME);
        if (currentStage != null)
        {
            Destroy(currentStage);
        }
        LoadScene(SceneName.InGame);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player == null)
        {
            var loadedPlayer = Resources.Load<GameObject>("Player");

            if (loadedPlayer != null)
            {
                player = Instantiate(loadedPlayer, Vector3.zero, Quaternion.identity);
            }
        }
        currentStageNum = stage;
        var loadedStage = Resources.Load<GameObject>($"Stages/Stage_{currentStageNum}");
        if(loadedStage != null)
        {
            currentStage = Instantiate(loadedStage, Vector3.zero, Quaternion.identity);
        }

        StartCoroutine(UIManager.Instance.FadeOut());
        isChanging = false;
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(UIManager.Instance.FadeIn());
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            this.player = player.GetComponent<PlayerController>();
            if (this.player != null)
            {
                this.player.onDeath += ReloadCurrentScene;
            }
            else
            {
                Debug.LogWarning("PlayerController를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("Player 태그를 가진 객체를 찾을 수 없습니다.");
        }
    }

    public void ReloadCurrentScene()
    {
        StartCoroutine(ReloadScene());
    }

    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("다음 씬이 없습니다. 마지막 씬입니다.");
        }
    }

    private IEnumerator ReloadScene()
    {
        yield return StartCoroutine(UIManager.Instance.FadeOut());

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }


}