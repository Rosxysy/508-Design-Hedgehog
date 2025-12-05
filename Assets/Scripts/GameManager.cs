using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using System.Xml.Serialization;
public class GameManager : MonoBehaviour
{

    public Image fadeImage;
    public float fadeDuration = 1f;

    private bool isFading = false;

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

    private void Awake()
    {
        // Make sure this stays when the scene changes
        DontDestroyOnLoad(gameObject);

        // Start fully transparent
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }
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

      // If player presses X while in the Den scene, reset the UI scran count
       if (Input.GetKeyDown(KeyCode.X) && SceneManager.GetActiveScene().name == DensceneName)
       {
           Debug.Log("X pressed in Den scene — resetting scran UI.");
            if (UICounter.Instance != null)
                UICounter.Instance.ResetScran();
       }

      // Check if energy is at 1 and in the yard scene.
      if (playerEnergy.currentEnergy == 1 && SceneManager.GetActiveScene().name == "SampleScene") //OR DAWN
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



            StartCoroutine(FadeToBlackThenLoadThenFade(2));
            //SceneManager.LoadScene(DensceneName);

        }


        //when in the den scene, if player collides with the door, load the Dawn scene. 
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);

            if (fadeImage != null)
            {
                Color c = fadeImage.color;
                float alpha = Mathf.Lerp(startAlpha, endAlpha, t);
                fadeImage.color = new Color(c.r, c.g, c.b, alpha);
            }

            yield return null;
        }
    }

    private IEnumerator FadeToBlackThenLoadThenFade(int sceneIndex)
    {
        isFading = true;

        // 1. Fade 0 -> 1 (clear to black)
        yield return StartCoroutine(Fade(0f, 1f));

       // yield return new WaitForSeconds(1);

        // 2. Load scene
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex);
        while (!op.isDone)
        {
            yield return null;
        }

        // Make sure image is still black after scene load
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 1f;
            fadeImage.color = c;
        }

        // 3. Fade 1 -> 0 (black to clear)
        yield return StartCoroutine(Fade(1f, 0f));

        isFading = false;
    }

}