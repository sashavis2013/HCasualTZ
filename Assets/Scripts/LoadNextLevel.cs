using System.Collections;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour
{

    public int NumberOfLevels;
    AsyncOperation loadingOperation;
    private bool loading = false;
    private int randomLevel;
    [SerializeField]
    private List<int> validChoices;
    public void LoadRandomLevel()
    {
        
        randomLevel = GenerateRand(randomLevel);
        GameManager.Instance.LastLevel = randomLevel;
        
        GameManager.Instance.CompletedLevelsList.Add(SceneManager.GetActiveScene().buildIndex);
        GameManager.Instance.Save();
        loadingOperation = SceneManager.LoadSceneAsync("Level_" + randomLevel);
        loading = true;
    }
    private int GenerateRand(int n)
    {
        n = validChoices[Random.Range(0, validChoices.Count)];

        return n;
    }

    public void LoadLevelFromSave(int level)
    {
        if (!GameManager.Instance.AllLevelsComplete)
        {
            loadingOperation = SceneManager.LoadSceneAsync("Level_" + level);
            loading = true;
        }
        else
            LoadRandomLevel();
        
    }

    public void LoadLevel()
    {
        if (!GameManager.Instance.AllLevelsComplete)
        {
            GameManager.Instance.LastLevel = SceneManager.GetActiveScene().buildIndex;
            GameManager.Instance.CurrentLevel = SceneManager.GetActiveScene().buildIndex;
            GameManager.Instance.CompletedLevelsList.Add(SceneManager.GetActiveScene().buildIndex);
            GameManager.Instance.Save();
            loadingOperation = SceneManager.LoadSceneAsync("Level_" + (SceneManager.GetActiveScene().buildIndex ));
            loading = true;
        }
        else
            LoadRandomLevel();
    }

    void Start()
    {
        validChoices = new List<int>();
        if (GameManager.Instance)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings-1; i++)
            {
                if (i != SceneManager.GetActiveScene().buildIndex-1)
                    validChoices.Add(i);
            }
        }
    }


}
