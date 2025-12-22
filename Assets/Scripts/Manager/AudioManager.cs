using System.Collections.Generic;
using UnityEngine;

public enum SFX
{
    ButtonPress,
    ButtonRelease,
    PlayerDie,
    GameOver,
    Victory,
    UIButton,
    COUNT
}

public enum BGM
{
    Title, 
    InGame,
    COUNT
}

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private Transform BGMTransform;
    [SerializeField] private Transform SFXTransform;

    private const string AUDIO_PATH = "Audio";

    private Dictionary<BGM, AudioSource> m_BGMPlayer = new Dictionary<BGM, AudioSource>();
    private AudioSource m_CurrentBGM;

    private Dictionary<SFX, AudioSource> m_SFXPlayer = new Dictionary<SFX, AudioSource>();

    protected override void Awake()
    {
        base.Awake();

        LoadBGMPLayer();
        LoadSFXPlayer();
        PlayBGM(BGM.Title);
    }

    public void SetBGMVolume(float value)
    {
        foreach(var bgm in m_BGMPlayer.Values)
        {
            bgm.volume = value;
        }
    }

    public void SetSFXVolume(float value)
    {
        foreach (var sfx in m_SFXPlayer.Values)
        {
            sfx.volume = value;
        }
    }

    private void LoadBGMPLayer()
    {
        for (int i = 0; i < (int)BGM.COUNT; i++)
        {
            var audioName = ((BGM)i).ToString();
            var pathstr = $"{AUDIO_PATH}/{audioName}";
            var audioClip = Resources.Load<AudioClip>(pathstr);
            if (audioClip == null)
            {
                Debug.LogError($"LoadBGMPlayer :: {audioName} clip does not exist");
                continue;
            }

            GameObject newBGMObj = new GameObject(audioName);
            var newAudioSource = newBGMObj.AddComponent<AudioSource>();
            newAudioSource.clip = audioClip;
            newAudioSource.loop = true;
            newAudioSource.playOnAwake = false;

            newBGMObj.transform.parent = BGMTransform;

            m_BGMPlayer[(BGM)i] = newAudioSource;
        }

        SetBGMVolume(0.1f);
    }

    private void LoadSFXPlayer()
    {
        for (int i = 0; i < (int)SFX.COUNT; i++)
        {
            var audioName = ((SFX)i).ToString();
            var pathstr = $"{AUDIO_PATH}/{audioName}";
            var audioClip = Resources.Load<AudioClip>(pathstr);
            if (audioClip == null)
            {
                Debug.LogError($"LoadSFXPlayer :: {audioName} clip does not exist");
                continue;
            }

            GameObject newSFXObj = new GameObject(audioName);
            var newAudioSource = newSFXObj.AddComponent<AudioSource>();
            newAudioSource.clip = audioClip;
            newAudioSource.loop = false;
            newAudioSource.playOnAwake = false;

            newSFXObj.transform.parent = SFXTransform;

            m_SFXPlayer[(SFX)i] = newAudioSource;
        }

        SetSFXVolume(0.1f);
    }

    public void PlayBGM(BGM bgm)
    {
        if(m_CurrentBGM == m_BGMPlayer[bgm] && m_CurrentBGM.isPlaying)
        {
            return;
        }
        if (m_CurrentBGM != null)
        {
            m_CurrentBGM.Stop();
            m_CurrentBGM = null;
        }

        if (!m_BGMPlayer.ContainsKey(bgm))
        {
            Debug.LogError($"PlayBGM :: invalid Clip name {bgm}");
            return;
        }

        m_CurrentBGM = m_BGMPlayer[bgm];
        m_CurrentBGM.Play();
    }

    public void PauseBGM()
    {
        if (m_CurrentBGM) m_CurrentBGM.Pause();
    }

    public void ResumeBGM()
    {
        if (m_CurrentBGM) m_CurrentBGM.UnPause();
    }

    public void StopBGM()
    {
        if (m_CurrentBGM) m_CurrentBGM.Stop();
    }

    public void PlaySFX(SFX sfx)
    {
        if (!m_SFXPlayer.ContainsKey(sfx))
        {
            Debug.LogError($"PlaySFX :: invalid Clip name {sfx}");
            return;
        }

        m_SFXPlayer[sfx].Play();
    }
}
