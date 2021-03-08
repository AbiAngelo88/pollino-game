using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LevelManager : UIManager
{

    [SerializeField] private TextMeshProUGUI pickedCollectablesText;
    [SerializeField] private Slider ecoPointsSlider;
    [SerializeField] private GameObject friendsContainer;
    [SerializeField] private GameObject enemiesContainer;
    [SerializeField] private GameObject collectablesContainer;
    [SerializeField] private GameObject joystick;
    [SerializeField] private GameObject jumpBtn;
    [SerializeField] private GameObject pauseBtn;

    public delegate void EndLevel(int collectablesScore, int ecoScore, int score);
    public static EndLevel EndLevelEmitter;

    public delegate void DestroyAI(GameObject ai);
    public static DestroyAI DestroyAIEmitter;

    public delegate void SaveAI(GameObject ai);
    public static SaveAI SaveAIEmitter;


    private Level currentLevel;
    private Level.Difficulty difficulty;

    private GameObject lastHurtedFriend;

    // Numeriche descrittive della partita
    private int pickedCollectables = 0;
    private int ecoPoints = 0;
    private int score = 0;
    private int totalCollectables, totalFriends, totalEnemies;
    private int destroyedEnemies, savedFriends;
    private bool hasHurtedFriend = false;
    
    

    private void Awake()
    {
        SetCurrentLevel();
    }

    void Start()
    {
        PlayerBodyManager.PickedCollectableEmitter += OnPickedCollectable;
        PlayerBodyManager.WinLevelEmitter += OnWinLevel;
        PlayerBodyManager.DefeatLevelEmitter += RestartLevel;
        PlayerBodyManager.ClimbOverFriendEmitter += OnClimbOverFriend;
        PlayerMovement.FriendCollisionEmitter += OnFriendCollision;
        PlayerMovement.EnemyCollisionEmitter += OnEnemyCollision;
        PlayerMovement.EnemyJumpEmitter += OnEnemyJump;
        ManageCorrectInteractions();

        // Generiamo le AI amiche
        GenerateAI(friendsContainer);

        // Generiamo le AI nemiche
        GenerateAI(enemiesContainer);

        // Generiamo i collectables
        GenerateCollectables();

        ecoPointsSlider.maxValue = totalEnemies + totalFriends;
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
        //if (pauseBtn)
        //{
        //    pauseBtn.SetActive(isMobileDevice);
        //}
    }

    private void HideControllers()
    {
        joystick.SetActive(false);
        pickedCollectablesText.gameObject.transform.parent.gameObject.SetActive(false);
        ecoPointsSlider.gameObject.SetActive(false);
        jumpBtn.SetActive(false);
        pauseBtn.SetActive(false);
    }

    private void GenerateCollectables()
    {
        Transform difficultyContainer;

        if (collectablesContainer.gameObject.transform.childCount < 1)
        {
            Debug.Log("Nel game object " + collectablesContainer.name + " della scena non sono presenti i contenitori di difficoltà.");
            return;
        }

        switch (difficulty)
        {
            case (Level.Difficulty.Easy):
                difficultyContainer = collectablesContainer.gameObject.transform.GetChild(0);
                break;
            case (Level.Difficulty.Medium):
                difficultyContainer = collectablesContainer.gameObject.transform.GetChild(1);
                break;
            case (Level.Difficulty.Hard):
                difficultyContainer = collectablesContainer.gameObject.transform.GetChild(2);
                break;
            default:
                difficultyContainer = collectablesContainer.gameObject.transform.GetChild(0);
                break;
        }

        totalCollectables = difficultyContainer.transform.childCount;

        if (totalCollectables == 0)
        {
            Debug.Log("Non ci sono SPAWN POINTS dentro il game object " + collectablesContainer.name + " per la difficoltà " + difficulty);
            return;
        }

        for (int i = 0; i < totalCollectables; i++)
        {
            Transform spawnPoint = difficultyContainer.GetChild(i);

            GameObject instance = Instantiate(Resources.Load("Chestnut", typeof(GameObject)) as GameObject, spawnPoint);
            instance.transform.parent = spawnPoint;

        }
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


        int totalSpawnPoints = difficultyContainer.transform.childCount;
        if (totalSpawnPoints == 0)
        {
            Debug.Log("Non ci sono SPAWN POINTS dentro il game object " + container.name + " per la difficoltà " + difficulty);
            return;
        }

        List<AI> aiToInstatiate = null;

        if (container.name == "Friends")
        {
            totalFriends = totalSpawnPoints;
            aiToInstatiate = currentLevel.GetFriends();
        }
        else
        {
            totalEnemies = totalSpawnPoints;
            aiToInstatiate = currentLevel.GetEnemies();
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
        pickedCollectablesText.text = pickedCollectables.ToString();
        Destroy(collectable);
    }

    private void OnFriendCollision(GameObject collision)
    {
        // Ottenere l'informazione di che eco malus ha l'AI
        AiManager aiManager = collision.gameObject.GetComponent<AiManager>();
        if (aiManager != null)
        {
            AI collidedAI = aiManager.GetAI();
            ecoPoints--;
            ecoPoints = ecoPoints <= 0 ? 0 : (ecoPoints >= (totalFriends + totalEnemies) ? (totalFriends + totalEnemies) : ecoPoints);
            ecoPointsSlider.value = ecoPoints;
            lastHurtedFriend = collision;
            hasHurtedFriend = true;

            DestroyAIEmitter?.Invoke(collision);
        }
    }

    private void OnEnemyCollision(GameObject collision)
    {
        pickedCollectables = 0;
        pickedCollectablesText.text = pickedCollectables.ToString();
    }

    private void OnEnemyJump(GameObject collision)
    {   
        // Ottenere l'informazione di che eco malus ha l'AI
        AiManager aiManager = collision.gameObject.GetComponent<AiManager>();
        if (aiManager != null)
        {
            AI collidedAI = aiManager.GetAI();
            ecoPoints++;
            ecoPoints = ecoPoints <= 0 ? 0 : (ecoPoints >= (totalFriends + totalEnemies) ? (totalFriends + totalEnemies) : ecoPoints);
            ecoPointsSlider.value = ecoPoints;
            lastHurtedFriend = collision;
            destroyedEnemies++;
            Debug.Log("Destroy " + collision.name + " tra 1 secondo");
            Destroy(collision.gameObject, .4f);
        }
    }

    private void OnClimbOverFriend(GameObject collision)
    {
        if (lastHurtedFriend == null || (collision.name != lastHurtedFriend.name))
        {
            AiManager aiManager = collision.gameObject.GetComponent<AiManager>();
            if (aiManager != null)
            {
                AI collidedAI = aiManager.GetAI();
                ecoPoints = ecoPoints + collidedAI.GetBaseDamage();
                ecoPoints = ecoPoints <= 0 ? 0 : (ecoPoints >= (totalFriends + totalEnemies) ? (totalFriends + totalEnemies) : ecoPoints);
                ecoPointsSlider.value = ecoPoints;
                savedFriends++;
                SaveAIEmitter?.Invoke(collision);
            }
            
        } 
        lastHurtedFriend = null;
    }


    private void OnWinLevel()
    {
        Time.timeScale = 0f;
        HideControllers();
        CalculateLevelScore();
        SaveLevelProgress();
        EndLevelEmitter?.Invoke(pickedCollectables, ecoPoints, score);
    }

    private void CalculateLevelScore()
    {
        score = 0;

        if (totalCollectables == pickedCollectables)
            score++;

        if (totalFriends == savedFriends && !hasHurtedFriend)
            score++;

        if (totalEnemies == destroyedEnemies)
            score++;
    }

    private void SaveLevelProgress()
    {
        PlayerData data = PersistentDataManager.Instance.GetCurrentPlayerData();
        Dictionary<Level.LevelID, Dictionary<Level.Difficulty, PlayerLevel>> levels = data.GetLevels();
        if(levels != null)
        {
            Debug.Log("Nessun livello salvato");
            Dictionary<Level.Difficulty, PlayerLevel> levelData;

            if(levels.TryGetValue(currentLevel.GetCode(), out levelData))
            {
                PlayerLevel level;

                if (levelData.TryGetValue(difficulty, out level))
                {
                    if(level.GetCollectablesScore() < pickedCollectables)
                        level.SetCollectablesScore(pickedCollectables);

                    if (level.GetEcoScore() < ecoPoints)
                        level.SetEcoScore(ecoPoints);

                    level.SetCompleted(true);
                }
                else
                {
                    level = new PlayerLevel(currentLevel.GetCode(), pickedCollectables, ecoPoints, score, true);
                }
            }
        }
        else
        {
            PlayerLevel level = new PlayerLevel(currentLevel.GetCode(), pickedCollectables, ecoPoints, score, true);
            Dictionary<Level.Difficulty, PlayerLevel> levelData = new Dictionary<Level.Difficulty, PlayerLevel>() { { difficulty, level } };
            levels = new Dictionary<Level.LevelID, Dictionary<Level.Difficulty, PlayerLevel>>() { { currentLevel.GetCode() , levelData } };
            data.SetLevels(levels);
        }

        PlayerSaver.Save(data);
    }

    private void OnDestroy()
    {
        PlayerBodyManager.PickedCollectableEmitter -= OnPickedCollectable;
        PlayerBodyManager.WinLevelEmitter -= OnWinLevel;
        PlayerBodyManager.DefeatLevelEmitter -= RestartLevel;
        PlayerBodyManager.ClimbOverFriendEmitter -= OnClimbOverFriend;
        PlayerMovement.FriendCollisionEmitter -= OnFriendCollision;
        PlayerMovement.EnemyCollisionEmitter -= OnEnemyCollision;
        PlayerMovement.EnemyJumpEmitter -= OnEnemyJump;
    }
}
