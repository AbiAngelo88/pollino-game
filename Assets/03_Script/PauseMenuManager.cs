using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : UIManager
{
    private GameObject pauseMenuUI;
    private bool gameIsPaused = false;

    private void Start()
    {
        pauseMenuUI = gameObject.transform.GetChild(0).gameObject;
        pauseMenuUI.SetActive(false);

        PauseClickEmitter += Pause;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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


    public void Pause()
    {
        Debug.Log("PAUSE");
        Time.timeScale = 0f;
        gameIsPaused = true;
        pauseMenuUI.SetActive(true);

    }

    public void Resume()
    {
        Debug.Log("RESUME");
        Time.timeScale = 1f;
        gameIsPaused = false;
        pauseMenuUI.SetActive(false);
    }

    private void OnDestroy()
    {
        PauseClickEmitter -= Pause;
    }

}
