using UnityEngine;
using UnityEngine.UI;
using System.IO;


[System.Serializable]
public class ScranSaveData
{
    public int scran;
}

public class UICounter : MonoBehaviour
{
    public static UICounter Instance { get; private set; }
    [SerializeField] private TMPro.TextMeshProUGUI scranText; // use TextMeshProUGUI for UI text
    int scran = 0;

    private string savePath;

    private void Awake()
    {
       if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        scran = 0;
        savePath = Path.Combine(Application.persistentDataPath, "keys.json");
        LoadScran();

    }

    void Start()
    {
        
        if (scranText != null)
            scranText.text = scran.ToString();
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void AddPoint()
    {
        scran += 1;
        if (scranText != null)
            scranText.text = scran.ToString();
        SaveScran();
    }


    public void SaveScran()
    {
        ScranSaveData data = new ScranSaveData();
        data.scran = scran;
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Keys saved to: " + savePath);
    }

    public void LoadScran()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            ScranSaveData data = JsonUtility.FromJson<ScranSaveData>(json);
            scran = data.scran;
            Debug.Log("Keys loaded from: " + savePath);
        }
        else
        {
            scran = 0;
        }
        if (scranText != null)
            scranText.text = scran.ToString();
    }

    
    public int GetKeys()
    {
        return scran;
    }

    public void ResetScran()
    {
        scran = 0;
        if (scranText != null)
            scranText.text = scran.ToString();
        SaveScran();
    }
}
