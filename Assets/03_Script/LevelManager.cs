using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : UIManager
{

    [SerializeField] private TextMeshProUGUI pickedCollectablesText;
    [SerializeField] private GameObject spawnPointsContainer;
    [SerializeField] private GameObject AIPrefab;
    [SerializeField] private GameObject joystick;
    [SerializeField] private GameObject jumpBtn;
    [SerializeField] private GameObject pauseBtn;

    public delegate void EndLevel(int collectablesScore, int ecoScore);
    public static EndLevel EndLevelEmitter;

    private int pickedCollectables = 0;
    private int totalSpawnPoints = 0;
    public int ecoPoints = 10;

    void Start()
    {
        PlayerBodyManager.PickedCollectableEmitter += OnPickedCollectable;
        PlayerBodyManager.WinLevelEmitter += OnWinLevel;
        PlayerBodyManager.DefeatLevelEmitter += RestartLevel;
        PlayerMovement.AICollisionEmitter += OnAICollision;
        ManageCorrectInteractions();
        GenerateAI();
    }

    private void ManageCorrectInteractions()
    {
        bool isMobileDevice = PersistentDataManager.Instance != null && PersistentDataManager.Instance.IsMobileDevice();

        if (joystick)
        {
            joystick.SetActive(isMobileDevice);
        }
        if (jumpBtn)
        {
            jumpBtn.SetActive(isMobileDevice);
        }
        if (pauseBtn)
        {
            pauseBtn.SetActive(isMobileDevice);
        }
    }

    private void HideControllers()
    {
        joystick.SetActive(false);
        pickedCollectablesText.gameObject.transform.parent.gameObject.SetActive(false);
        jumpBtn.SetActive(false);
        pauseBtn.SetActive(false);
    }


    private void GenerateAI()
    {

        totalSpawnPoints = spawnPointsContainer.gameObject.transform.childCount;
        if (totalSpawnPoints == 0)
        {
            Debug.Log("SPAWN POINTS: " + totalSpawnPoints);
            return;
        }
        for(int i = 0; i < totalSpawnPoints; i++)
        {
            Transform spawnPoint = spawnPointsContainer.gameObject.transform.GetChild(i);
            GameObject instance = Instantiate(AIPrefab, spawnPoint);
            instance.transform.parent = spawnPoint;
        }
    }

    private void OnPickedCollectable(GameObject collectable)
    {
        pickedCollectables += 1;
        Destroy(collectable);
        pickedCollectablesText.text = pickedCollectables.ToString();
    }

    private void OnAICollision(GameObject collectable)
    {
        ecoPoints -= 1;
        //pickedCollectablesText.text = pickedCollectables.ToString();
    }
    private void OnWinLevel()
    {
        Time.timeScale = 0f;
        HideControllers();
        EndLevelEmitter?.Invoke(pickedCollectables, ecoPoints);
    }

        private void OnDestroy()
    {
        PlayerBodyManager.PickedCollectableEmitter -= OnPickedCollectable;
        PlayerMovement.AICollisionEmitter -= OnAICollision;
        PlayerBodyManager.WinLevelEmitter -= OnWinLevel;
        PlayerBodyManager.DefeatLevelEmitter -= RestartLevel;
    }
}
