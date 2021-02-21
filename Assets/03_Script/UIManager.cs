using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public delegate void RightBtnTouch();
    public static event RightBtnTouch OnRightBtnTouch;

    public delegate void PauseClick();
    public static event PauseClick PauseClickEmitter;

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    public void LoadLevel(Loader.Scene scene)
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

}
