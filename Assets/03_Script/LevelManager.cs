using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : UIManager
{

    [SerializeField] private TextMeshProUGUI pickedCollectablesText;
    [SerializeField] private GameObject friendsContainer;
    [SerializeField] private GameObject enemiesContainer;
    //[SerializeField] private GameObject AIPrefab;
    [SerializeField] private GameObject joystick;
    [SerializeField] private GameObject jumpBtn;
    [SerializeField] private GameObject pauseBtn;

    public delegate void EndLevel(int collectablesScore, int ecoScore);
    public static EndLevel EndLevelEmitter;

    private int pickedCollectables = 0;
    public int ecoPoints = 10;
    private Level currentLevel;
    private Level.Difficulty difficulty;
    

    private void Awake()
    {
        SetCurrentLevel();
    }

    void Start()
    {
        PlayerBodyManager.PickedCollectableEmitter += OnPickedCollectable;
        PlayerBodyManager.WinLevelEmitter += OnWinLevel;
        PlayerBodyManager.DefeatLevelEmitter += RestartLevel;
        PlayerMovement.AICollisionEmitter += OnAICollision;
        ManageCorrectInteractions();

        // Generiamo le AI amiche
        GenerateAI(friendsContainer);

        // Generiamo le AI nemiche
        GenerateAI(enemiesContainer);
    }

    private void SetCurrentLevel()
    {
        currentLevel = PersistentDataManager.GetLevel(SceneManager.GetActiveScene().name);
        difficulty = Level.Difficulty.Easy;
        //Debug.Log("CURRENT LEVEL " + currentLevel.GetCode());
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


    private void GenerateAI(GameObject container)
    {
        Transform difficultyContainer;

        if (container.gameObject.transform.childCount < 1)
        {
            Debug.Log("Nel game object " + container.name + " della scena non sono presenti i contenitori di difficoltà delle AI");
            return;
        }

        switch (difficulty)
        {
            case (Level.Difficulty.Easy):
                difficultyContainer = container.gameObject.transform.GetChild(0);
                break;
            case (Level.Difficulty.Medium):
                difficultyContainer = container.gameObject.transform.GetChild(1);
                break;
            case (Level.Difficulty.Hard):
                difficultyContainer = container.gameObject.transform.GetChild(2);
                break;
            default:
                difficultyContainer = container.gameObject.transform.GetChild(0);
                break;
        }

        List<AI> aiToInstatiate = container.name == "Friends" ? currentLevel.GetFriends() : currentLevel.GetEnemies();

        int totalSpawnPoints = difficultyContainer.transform.childCount;

        if (totalSpawnPoints == 0)
        {
            Debug.Log("Non ci sono SPAWN POINTS dentro il game object " + container.name + " per la difficoltà " + difficulty);
            return;
        }
        
        for(int i = 0; i < totalSpawnPoints; i++)
        {
            Transform spawnPoint = difficultyContainer.GetChild(i);

            //Ottengo l'informazione del prefab da istanziare
            SpawnPointManager spManager = spawnPoint.gameObject.GetComponent<SpawnPointManager>();

            AI toInstantiate = aiToInstatiate.Find(ai => ai.GetCode() == spManager.prefabName);

            if(toInstantiate == null)
            {
                Debug.Log("Non è possibile istanziare " + spManager.prefabName + " perchè non è previsto dalla configurazione del livello");
                Debug.Log("Per istanziarlo è necessario aggiungerlo alla lista dei " + container.name + " del livello " + currentLevel);
                return;
            }

            GameObject instance = Instantiate(Resources.Load(toInstantiate.GetPrefabName(), typeof(GameObject)) as GameObject, spawnPoint);

            // Settare nelle istanze le varie caratteristiche delle AI: canFly, isEnemy, isFriend, baseDamage
            AiManager aiManager = instance.GetComponent<AiManager>();
            aiManager.SetAI(toInstantiate);
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
