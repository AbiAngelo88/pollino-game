using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PersistentDataManager : MonoBehaviour
{
    private PlayerData currentPlayer;
    private AudioSource audioSource;
    [SerializeField] private AudioMixer audioMixer;
    private bool isMobile = false;

    public static PersistentDataManager Instance { get; private set; }


    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        isMobile = Application.isMobilePlatform;

        Debug.Log("NAME " + SystemInfo.deviceName);
        Debug.Log("TYPE " + SystemInfo.deviceType);
        Debug.Log("MODEL " + SystemInfo.deviceModel);
        Debug.Log("IS MOBILE " + Application.isMobilePlatform);

        this.currentPlayer = PlayerSaver.Load();
        OnVolumeChange(currentPlayer.getVolume());
    }

    void Start()
    {
        PlayerSaver.OnLoadedPlayer += OnPlayerDataChange;
        PlayerSaver.OnPlayerSaved += OnPlayerDataChange;
        MenuSceneManager.VolumeChangeEmitter += OnVolumeChange;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnVolumeChange(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public bool IsMobileDevice()
    {
        return this.isMobile;
    }


    public PlayerData GetCurrentPlayerData()
    {
        return this.currentPlayer;
    }

    private void OnPlayerDataChange(PlayerData data)
    {
        this.currentPlayer = data;
    }

    private void OnDestroy()
    {
        PlayerSaver.OnLoadedPlayer -= OnPlayerDataChange;
        PlayerSaver.OnPlayerSaved -= OnPlayerDataChange;
        MenuSceneManager.VolumeChangeEmitter -= OnVolumeChange;
    }

}
