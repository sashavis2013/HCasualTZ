using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameScript : MonoBehaviour
{

    public int CurrentLevel;
    public bool AllLevelsComplete;
    public int NumberOfLevels;
    private AsyncOperation loadingOperation;
    private int randomLevel;
    public int LastLevel;
    private List<int> validChoices;
    void Start()
    {
        LoadJsonData(this);
        validChoices = new List<int>();
        NumberOfLevels = SceneManager.sceneCountInBuildSettings-1;
        for (int i = 1; i < NumberOfLevels; i++)
        {
            if(i!=LastLevel)
                validChoices.Add(i);
        }
        
        if (!AllLevelsComplete)
        {
            loadingOperation = SceneManager.LoadSceneAsync("Level_" + CurrentLevel);
        }
        else
        {
            LoadRandomLevel();

        }
    }

    public void LoadRandomLevel()
    {

        randomLevel = GenerateRand(randomLevel);
        loadingOperation = SceneManager.LoadSceneAsync("Level_" + randomLevel);
    }

    private int GenerateRand(int n)
    {
        n = validChoices[Random.Range(0, validChoices.Count)];

        return n;
    }

    private void LoadJsonData(StartGameScript startGameScript)
    {
        if (FileManager.LoadFromFile("SaveData.dat", out var json))
        {
            SaveData sd = new SaveData();
            sd.LoadFromJson(json);
            startGameScript.LoadFromSaveData(sd);
            Debug.Log("Loaded save data");
        }
    }

    public void LoadFromSaveData(SaveData saveData)
    {
        AllLevelsComplete = saveData.AllLevelsComplete;
        CurrentLevel = saveData.Level;
    }
}
