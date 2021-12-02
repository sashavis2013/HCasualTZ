using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveable
{

    public static GameManager Instance;
    public TextMeshProUGUI AllItemsText;
    public TextMeshProUGUI DestroyedItemsText;
    public TextMeshProUGUI CompletedLevelsText;
    public LoadNextLevel LevelManager;

    private float destroyedItems;
    private float allItems;
    private bool gameStarted=false;
    private bool levelComplete=false;
    public List<int> CompletedLevelsList;
    public int LastLevel;
    public int CurrentLevel;
    public GameObject GameUi;
    public GameObject LevelCompleteUi;
    public bool AllLevelsComplete;
    public float DestroyedItems
    {
        get => destroyedItems;
        set
        {
            if (destroyedItems == value) return;
            destroyedItems = value;
            OnScoreChange?.Invoke(destroyedItems);
        }
    } 
    public float AllItems
    {
        get => allItems;
        set
        {
            if (allItems == value) return;
            allItems = value;
            OnScoreChange?.Invoke(allItems);
        }
    }

    public bool GameStarted
    {
        get => gameStarted;
        set
        {
            if (gameStarted == value) return;
            gameStarted = value;
            OnFirstTap?.Invoke(gameStarted);
        }
    }


    public delegate void OnScoreChangeDelegate(float newVal);
    public event OnScoreChangeDelegate OnScoreChange;
    public delegate void OnFirstTapDelegate(bool newVal);
    public event OnFirstTapDelegate OnFirstTap;

    void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
            
        }
    }


    


    void Start()
    {
        
        this.OnScoreChange += ScoreChangeHandler;
        this.OnFirstTap += GameStateHandler;
        LoadJsonData(this);
        if (CompletedLevelsList.Count > 0)
        {

            for (int i = 0; i < CompletedLevelsList.Count; i++)
            {
                CompletedLevelsText.text+= CompletedLevelsList[i].ToString() + " ";
            }

        }
            
    }


    private void ScoreChangeHandler(float newVal)
    {
        AllItemsText.text = AllItems.ToString();
        DestroyedItemsText.text = DestroyedItems.ToString();
    }
    private void GameStateHandler(bool newVal)
    {
        GameUi.SetActive(!GameUi.activeInHierarchy);
    }

    private void OnApplicationQuit()
    {
        CompletedLevelsList.Clear();
        SaveJsonData(this);
    }

    public void Save()
    {
        SaveJsonData(this);
    }

    void Update()
    {
        if (gameStarted)
        {
            if (destroyedItems == allItems)
            {
                LastLevel = SceneManager.GetActiveScene().buildIndex;
                levelComplete = true;
                GameUi.SetActive(false);
                LevelCompleteUi.SetActive(true);
            }
        }

        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings-1 && !AllLevelsComplete&&levelComplete)
        {
            AllLevelsComplete = true;
            SaveJsonData(this);
        }
    }


    private static void SaveJsonData(GameManager gameManager)
    {
        SaveData sd = new SaveData();
        gameManager.PopulateSaveData(sd);
        if (FileManager.WriteToFile("SaveData.dat", sd.ToJson()))
        {
            Debug.Log("Saved Successfully");
        }
    }

    public void PopulateSaveData(SaveData saveData)
    {
        saveData.CompletedLevelsList = CompletedLevelsList;

        saveData.AllLevelsComplete = AllLevelsComplete;
        saveData.Level = CurrentLevel;
        saveData.LastLevel = LastLevel;
    }

    private void LoadJsonData(GameManager gameManager)
    {
        if (FileManager.LoadFromFile("SaveData.dat", out var json))
        {
            SaveData sd = new SaveData();
            sd.LoadFromJson(json);
            gameManager.LoadFromSaveData(sd);
            Debug.Log("Loaded save data");
        }
    }

    public void LoadFromSaveData(SaveData saveData)
    {
        AllLevelsComplete = saveData.AllLevelsComplete;
        CurrentLevel = saveData.Level;
        LastLevel = saveData.LastLevel;
        CompletedLevelsList = saveData.CompletedLevelsList;

    }
    
}
