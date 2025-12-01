using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    private bool isFading = false;

    void Awake()
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isFading)
        {
            // Load scene 0 with fade
            StartCoroutine(FadeToBlackThenLoadThenFade(0));
        }
    }

    private IEnumerator FadeToBlackThenLoadThenFade(int sceneIndex)
    {
        isFading = true;

        // 1. Fade 0 -> 1 (clear to black)
        yield return StartCoroutine(Fade(0f, 1f));

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
}