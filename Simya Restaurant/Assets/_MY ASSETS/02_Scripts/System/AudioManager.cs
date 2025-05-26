using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Mixer ����")]
    public AudioMixer audioMixer;
    public AudioMixerGroup bgmMixer;
    public AudioMixerGroup sfxMixer;

    [Header("BGM ����")]
    public AudioClip[] bgmClips;
    public float bgmVolume;
    private AudioSource bgmPlayer;

    [Header("SFX ����")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int sfxChannels;
    private AudioSource[] sfxPlayers;
    private int sfxChannelIndex;

    [Header("Vol UI")]
    [SerializeField] private Slider masterVolSlider;
    [SerializeField] private Slider bgmVolSlider;
    [SerializeField] private Slider sfxVolSlider;

    /// <summary>
    /// ���� BGM ����
    /// </summary>
    /// <returns></returns>
    public enum BGM
    {
        BGM_Village,
        BGM_Forest
    }

    /// <summary>
    /// ���� ȿ���� ����
    /// </summary>
    /// <returns></returns>
    public enum SFX
    {
        FlowerMonsterExplodes,
        FlowerMonsterSpitting,
        Mandla_long,
        Mandla_Short,
        BasicAttack_1,
        BasicAttack_2,
        Bush,
        BuyItem,
        End,
        GameStart,
        MonsterDeath,
        PotionCrafting_1,
        PotionCrafting_2,
        SkipConversation,
        UsePotion
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitSliders();

        masterVolSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmVolSlider.onValueChanged.AddListener(SetBgmVolume);
        sfxVolSlider.onValueChanged.AddListener(SetSfxVolume);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetBgmForScene(scene.name);
    }

    /// <summary>
    /// �� �̸��� ���� ������ BGM ����
    /// </summary>
    /// <param name="sceneName">���� �� �̸�</param>
    private void SetBgmForScene(string sceneName)
    {
        AudioClip selectedBgm = null;

        if (sceneName == "New Home" || sceneName == "New Village" || sceneName == "New Title")
        {
            selectedBgm = bgmClips[(int)BGM.BGM_Village];
        }
        else if (sceneName == "New Forest")
        {
            selectedBgm = bgmClips[(int)BGM.BGM_Forest];
        }
        else
        {
            Debug.LogWarning($"'{sceneName}'�� ���� BGM ������ �����ϴ�. �⺻���� ����մϴ�.");
            selectedBgm = bgmClips[(int)BGM.BGM_Village];
        }

        if (selectedBgm != null && bgmPlayer.clip != selectedBgm)
        {
            bgmPlayer.Stop();
            bgmPlayer.clip = selectedBgm;
            bgmPlayer.Play();
        }
    }

    /// <summary>
    /// �����̴� �ʱ�ȭ (����� �ͼ��� ���� �� �ݿ�)
    /// </summary>
    /// <returns></returns>
    void InitSliders()
    {
        float volume;

        if (audioMixer.GetFloat("MasterVol", out volume))
        {
            masterVolSlider.value = Mathf.Pow(10, volume / 20);
        }

        if (audioMixer.GetFloat("BGMVol", out volume))
        {
            bgmVolSlider.value = Mathf.Pow(10, volume / 20);
        }

        if (audioMixer.GetFloat("SFXVol", out volume))
        {
            sfxVolSlider.value = Mathf.Pow(10, volume / 20);
        }
    }

    /// <summary>
    /// ����� �÷��̾� �ʱ�ȭ
    /// </summary>
    /// <returns></returns>
    void Init()
    {
        // BGM Player �ʱ�ȭ
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.outputAudioMixerGroup = bgmMixer;

        // SFX Player �ʱ�ȭ
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[sfxChannels];
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].volume = sfxVolume;
            sfxPlayers[index].outputAudioMixerGroup = sfxMixer;
        }
    }

    /// <summary>
    /// BGM ���
    /// </summary>
    /// <returns></returns>
    public void PlayBgm(int bgmIndex)
    {
        if(bgmIndex < 0 || bgmIndex >= bgmClips.Length)
        {
            return;
        }

        if (bgmPlayer.clip != bgmClips[bgmIndex])
        {
            bgmPlayer.clip = bgmClips[bgmIndex];
            bgmPlayer.Play();
        }
    }

    /// <summary>
    /// SFX ��� 
    /// </summary>
    /// <returns></returns>
    public void PlaySfx(SFX sfx)
    {
        int sfxIndex = (int)sfx;

        if (sfxIndex < 0 || sfxIndex >= sfxClips.Length)
        {
            return;
        }

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + sfxChannelIndex) % sfxPlayers.Length;

            if (!sfxPlayers[loopIndex].isPlaying)
            {
                sfxChannelIndex = loopIndex;
                sfxPlayers[loopIndex].clip = sfxClips[sfxIndex];
                sfxPlayers[loopIndex].Play();
                break;
            }
        }
    }


    /// <summary>
    ///  �����̴� ���� ���ú��� ���� �� ����
    /// </summary>
    /// <param name="volume"></param>
    public void SetVolume(string volumeName, float volume)
    {
        volume = Mathf.Clamp(volume, 0.001f, 1f);
        audioMixer.SetFloat(volumeName, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(volumeName, volume);
        PlayerPrefs.Save();
    }

    public void SetMasterVolume(float volume) => SetVolume("MasterVol", volume);
    public void SetBgmVolume(float volume) => SetVolume("BGMVol", volume);
    public void SetSfxVolume(float volume) => SetVolume("SFXVol", volume);
}
