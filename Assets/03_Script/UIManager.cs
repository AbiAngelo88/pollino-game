using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public delegate void RightBtnTouch();
    public static event RightBtnTouch OnRightBtnTouch;

    public delegate void PauseClick();
    public static event PauseClick PauseClickEmitter;

    private void Start()
    {
        PlayerBodyManager.DefeatLevelEmitter += RestartLevel;
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    public static void LoadLevel(Loader.Scene scene)
    {
        Loader.loadScene(scene);
    }

    public void Jump()
    {
        OnRightBtnTouch?.Invoke();
    }

    public void PauseGame()
    {
        PauseClickEmitter?.Invoke();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        Loader.loadScene(Loader.Scene.MenuScene);
    }

    private void OnDestroy()
    {
        PlayerBodyManager.DefeatLevelEmitter -= RestartLevel;
    }

}
