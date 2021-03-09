using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class EndLevelUIManager : UIManager
{
    [SerializeField] private TextMeshProUGUI collectablesScoreText;
    [SerializeField] private TextMeshProUGUI ecoScoreText;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private GameObject endLevelPanel;
    [SerializeField] private GameObject[] trophies;
    
    void Start()
    {
        LevelManager.EndLevelEmitter += OnEndLevel;
        endLevelPanel.SetActive(false);
    }

    private void OnEndLevel(int collectablesScore, int ecoScore, int score, string nickname)
    {
        Debug.Log("End Level");
        Debug.Log(collectablesScore);
        Debug.Log(ecoScore);
        endLevelPanel.SetActive(true);
        collectablesScoreText.text = collectablesScore.ToString();
        ecoScoreText.text = ecoScore.ToString();
        playerNameText.text = nickname;

        ShowTrophies(score);
    }

    private void ShowTrophies(int score)
    {
        for(int i = 0; i < score; i++)
        {
            if(trophies[i] != null)
            {
                trophies[i].transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    private void OnDestroy()
    {
        LevelManager.EndLevelEmitter -= OnEndLevel;
    }
}
