using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
   
    void Start()
    {
        
    }

    
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
             Cursor.visible = true;
             Cursor.lockState = CursorLockMode.None;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);

    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
