using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;

    public delegate void RightBtnTouch();
    public static event RightBtnTouch OnRightBtnTouch;

    public void PlayGame()
    {
        Loader.loadScene(Loader.Scene.Level_01);
    }

    public void LoadLevel(Loader.Scene scene)
    {
        Loader.loadScene(scene);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume() {
        Time.timeScale = 1f;
        gameIsPaused = false;
        pauseMenuUI.SetActive(false);
    }

    public void Pause() {
        Time.timeScale = 0f;
        gameIsPaused = true;
        pauseMenuUI.SetActive(true);
            
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        Loader.loadScene(Loader.Scene.MenuScene);
    }

    public void Jump()
    {
        Debug.Log("JUMP UI");
        OnRightBtnTouch?.Invoke();
    }

}
