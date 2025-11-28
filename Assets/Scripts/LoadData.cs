using UnityEngine;
using System.IO;
public class LoadData : MonoBehaviour
{
    public SaveData saveData = new SaveData();
    private string savePath;

    void Start()
    {
        savePath = Path.Combine(Application.persistentDataPath, "saveData.json");
    }

    public void SaveToJson()
    {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game saved to: " + savePath);
    }

    public void LoadFromJson()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            saveData = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Game loaded from: " + savePath);
        }
        else
        {
            Debug.LogWarning("Save file not found!");
        }
    }
}
