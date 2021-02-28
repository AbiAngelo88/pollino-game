using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class EndLevelUIManager : UIManager
{
    [SerializeField] private TextMeshProUGUI collectablesScoreText;
    [SerializeField] private TextMeshProUGUI ecoScoreText;
    [SerializeField] private GameObject endLevelPanel;
    
    void Start()
    {
        LevelManager.EndLevelEmitter += OnEndLevel;
        endLevelPanel.SetActive(false);
    }

    private void OnEndLevel(int collectablesScore, int ecoScore)
    {
        Debug.Log("End Level");
        Debug.Log(collectablesScore);
        Debug.Log(ecoScore);
        endLevelPanel.SetActive(true);
        collectablesScoreText.text = collectablesScore.ToString();
        ecoScoreText.text = ecoScore.ToString();
    }

    private void OnDestroy()
    {
        LevelManager.EndLevelEmitter -= OnEndLevel;
    }
}
