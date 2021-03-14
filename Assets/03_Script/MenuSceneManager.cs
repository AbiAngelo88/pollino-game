using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSceneManager : CoreSceneManager
{
    [SerializeField] private GameObject menuContainer;
    [SerializeField] private GameObject firstGameCanvas;

    [SerializeField] private InputField optionsNicknameField;
    [SerializeField] private Slider volumeInput;

    [SerializeField] InputField firstGameNicknameField;

    public delegate void VolumeChange(float volume);
    public static event VolumeChange VolumeChangeEmitter;

    private float volume;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
        AudioHelper.PlayBackGroundMusic(AudioHelper.Sounds.Menu);
        OnPlayerDataChange(PersistentDataManager.Instance.GetCurrentPlayerData());
        PlayerSaver.OnPlayerSaved += OnPlayerDataChange;
    }

    private void OnPlayerDataChange(PlayerData data)
    {
        bool isFirstGame = data.GetIsFirstGame();
        ActivateCanvas(isFirstGame);
        RefreshOptions(data);
    }

    private void  RefreshOptions(PlayerData data) {
        if (volumeInput != null)
        {
            volumeInput.value = data.GetVolume();
        }

        if (optionsNicknameField != null)
        {
            optionsNicknameField.text = data.GetNickname();
        }
    }

    private void ActivateCanvas(bool isFirstGame)
    {
        if (menuContainer != null)
            menuContainer.SetActive(!isFirstGame);

        if (firstGameCanvas != null)
            firstGameCanvas.SetActive(isFirstGame);
    }


    public void SaveOptions()
    {

        if (optionsNicknameField == null || optionsNicknameField.text == null || optionsNicknameField.text == "")
        {
            Debug.Log("Nessun nickname digitato");
            return;
        }

        PlayerData data = PersistentDataManager.Instance.GetCurrentPlayerData();
        if (data != null)
        {
            data.SetIsFirstGame(false);
            data.SetNickname(optionsNicknameField.text);
            data.SetVolume(volume);
            PlayerSaver.Save(data);
        }
    }

    public void SetVolume(float value)
    {
        this.volume = value;
        VolumeChangeEmitter?.Invoke(this.volume);
        AudioHelper.PlayOneShotSound(AudioHelper.Sounds.VolumeChange);
    }

    public void SaveNickname()
    {

        if (firstGameNicknameField == null || firstGameNicknameField.text == null || firstGameNicknameField.text == "")
        {
            Debug.Log("Nessun nickname digitato");
            return;
        }

        PlayerData data = PersistentDataManager.Instance.GetCurrentPlayerData();

        if (data != null)
        {
            data.SetIsFirstGame(false);
            data.SetNickname(firstGameNicknameField.text);
            PlayerSaver.Save(data);
        }
    }

    public void PlayGame()
    {
        Loader.loadScene(Loader.Scene.Level_01);
    }

    private void OnDestroy()
    {
        PlayerSaver.OnPlayerSaved -= OnPlayerDataChange;
    }
}
