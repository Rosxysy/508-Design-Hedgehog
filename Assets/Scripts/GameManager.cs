using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    public SaveData saveData = new SaveData();
    private string savePath;

    [Header("Scene Settings")]
    [Tooltip("The name of the scene where the key collection happens.")]
    public string YardsceneName = "SampleScene";
    [Tooltip("The name of the scene to load once conditions are met.")]
    public string DensceneName = "Den";

    [Header("Player")]
    [Tooltip("Reference to the PlayerEnergy component. If left empty, it will find one in the scene at Start.")]
    public PlayerEnergy playerEnergy;

    private bool hasTriggeredSceneChange = false;

    // The number the energy bar reachs to reset back to the Den scene
    public int requiredEnergy;
    public int DrainEnergyByDistance() => requiredEnergy;

    [Obsolete]
    public void Start()
    {
        saveData = new SaveData(); // Reset all values

        savePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        if (File.Exists(savePath))
            File.Delete(savePath); // Optional: Delete old save

        SaveToJson();
        Debug.Log("New game started. Save data reset.");

        if (playerEnergy == null)
             playerEnergy = FindObjectOfType<PlayerEnergy>();

    }


    private void SaveToJson()
    {
        savePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game saved to: " + savePath);
    }

    public void RestartGame()
    {
        // Reload the current scene to restart the game
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }


    void Update()
    {
         Debug.Log("Current Scene: " + SceneManager.GetActiveScene());

      // Check if energy is at 1 and in the yard scene.
      if (playerEnergy.currentEnergy == 1 && SceneManager.GetActiveScene().name == "SampleScene")
        {
            Debug.Log("Condition met! Loading WinScene..."); // Debug message before loading
            SceneManager.LoadScene("Den");
        }
        // If we're in the yard scene and player runs out of energy, load the Den.
       if (!hasTriggeredSceneChange
            && SceneManager.GetActiveScene().name == YardsceneName
            && playerEnergy != null
            && playerEnergy.currentEnergy <= 0)
        {
            Debug.Log("Player energy depleted. Loading " + DensceneName + "...");
            hasTriggeredSceneChange = true;
            SceneManager.LoadScene(DensceneName);
        }
    }
}