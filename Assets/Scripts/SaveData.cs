using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int Level;
    public int LastLevel;
    public bool AllLevelsComplete;
    public List<int> CompletedLevelsList;
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
    public void LoadFromJson(string Json)
    {
        JsonUtility.FromJsonOverwrite(Json, this);
    }
}

public interface ISaveable
{
    void PopulateSaveData(SaveData saveData);
    void LoadFromSaveData(SaveData saveData);
}