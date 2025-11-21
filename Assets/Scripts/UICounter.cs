using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class UICounter : MonoBehaviour
{
    public static UICounter Instance { get; private set; }
    [SerializeField] private TMPro.TextMeshProUGUI scranText; // use TextMeshProUGUI for UI text
    int scran = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        scran = 0;
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
    }
}
