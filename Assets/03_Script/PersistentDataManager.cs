using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PersistentDataManager : MonoBehaviour
{
    private PlayerData currentPlayer;
    
    private bool isMobile = false;

    public static PersistentDataManager Instance { get; private set; }

    public static List<Level> levels = new List<Level>{
        new Level(Level.LevelID.Level_01, false, new List<AI> { new AI(AI.AiCodes.Pyromaniac, true, false, 1, false, false, "Pyromaniac") },
            new List<AI> { new AI(AI.AiCodes.Fox, true, false, 1, false, true, "Fox"), new AI(AI.AiCodes.Boar, true, false, 1, false, true, "Boar")}
        )
    };


    public static Level GetLevel(string levelCode)
    {
        return levels.Find(l => l.GetCode().ToString().Equals(levelCode));
    }

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
        
    }

    void Start()
    {
        PlayerSaver.OnLoadedPlayer += OnPlayerDataChange;
        PlayerSaver.OnPlayerSaved += OnPlayerDataChange;
        
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
    }

}
