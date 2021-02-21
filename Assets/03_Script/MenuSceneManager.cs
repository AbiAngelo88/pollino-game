using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject menuContainer;
    [SerializeField] private GameObject firstGameCanvas;

    [SerializeField] private InputField optionsNicknameField;
    [SerializeField] private Slider volumeInput;

    [SerializeField] InputField firstGameNicknameField;

    public delegate void VolumeChange(float volume);
    public static event VolumeChange VolumeChangeEmitter;

    private float volume;

    void Start()
    {

        OnPlayerDataChange(PersistentDataManager.Instance.GetCurrentPlayerData());
        PlayerSaver.OnPlayerSaved += OnPlayerDataChange;
    }

    private void OnPlayerDataChange(PlayerData data)
    {
        bool isFirstGame = data.getIsFirstGame();
        ActivateCanvas(isFirstGame);
        RefreshOptions(data);
    }

    private void  RefreshOptions(PlayerData data) {
        if (volumeInput != null)
        {
            volumeInput.value = data.getVolume();
        }

        if (optionsNicknameField != null)
        {
            optionsNicknameField.text = data.getNickname();
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
            data.setIsFirstGame(false);
            data.setNickname(optionsNicknameField.text);
            data.setVolume(volume);
            PlayerSaver.Save(data);
        }
    }

    public void SetVolume(float value)
    {
        this.volume = value;
        VolumeChangeEmitter?.Invoke(this.volume);
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
            data.setIsFirstGame(false);
            data.setNickname(firstGameNicknameField.text);
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
