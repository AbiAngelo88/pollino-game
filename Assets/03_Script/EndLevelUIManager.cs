using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class EndLevelUIManager : UIManager
{
    [SerializeField] private TextMeshProUGUI collectablesScoreText;
    [SerializeField] private Slider ecoSlider;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private GameObject endLevelPanel;
    [SerializeField] private GameObject[] trophies;
    
    void Start()
    {
        LevelManager.EndLevelEmitter += OnEndLevel;
        endLevelPanel.SetActive(false);
    }

    private void OnEndLevel(int collectablesScore, int ecoScore, int maxEcoPoints, int trophies, string nickname)
    {
        endLevelPanel.SetActive(true);
        collectablesScoreText.text = collectablesScore.ToString();
        ecoSlider.maxValue = maxEcoPoints;
        ecoSlider.value = ecoScore;
        playerNameText.text = nickname;

        StartCoroutine(ShowTrophies(collectablesScore, ecoScore, trophies));
    }

    private IEnumerator ShowTrophies(int collectablesScore, int ecoScore, int trophiesScore)
    {

        yield return new WaitForSeconds(1f);

        // Animazione Collectables
        for (int i = collectablesScore; i >= 0; i--)
        {
            yield return new WaitForSeconds(.05f);
            collectablesScoreText.text = i.ToString();
        }

        yield return new WaitForSeconds(.5f);

        // Animazione slider
        for (int i = ecoScore; i >= 0; i--)
        {
            yield return new WaitForSeconds(.01f);
            ecoSlider.value = i;
        }
        
        // Animazione trofei
        for (int i = 0; i < trophiesScore; i++)
        {
            yield return new WaitForSeconds(1f);

            if (trophies[i] != null)
            {
                GameObject t = trophies[i].transform.GetChild(0).gameObject;
                t.GetComponent<Animator>().Play("Trophy_Idle");
            }
        }
    }

    private void OnDestroy()
    {
        LevelManager.EndLevelEmitter -= OnEndLevel;
    }
}
