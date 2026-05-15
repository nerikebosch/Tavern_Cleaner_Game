using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // CRITICAL: This line allows us to use delays (Coroutines)!

public class MainMenuController : MonoBehaviour
{
    public string gameSceneName = "Tavern_Scene";

    [Header("Audio")]
    public AudioSource clickSound; // The sound we want to play
    public float delayBeforeLoad = 0.3f; // How long to wait before loading the scene

    [Header("UI Panels")]
    public GameObject instructionsPanel; // The new instructions pop-up!

    public void StartGame()
    {
        // 1. Play the sound instantly
        PlayClickSound();

        // 2. Start the timer to delay the scene load
        StartCoroutine(LoadSceneWithDelay());
    }

    IEnumerator LoadSceneWithDelay()
    {
        // Wait for 0.3 seconds...
        yield return new WaitForSeconds(delayBeforeLoad);

        // ...THEN load the Tavern!
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        PlayClickSound();
        StartCoroutine(QuitWithDelay());
    }

    IEnumerator QuitWithDelay()
    {
        yield return new WaitForSeconds(delayBeforeLoad);
        Debug.Log("Game is Exiting!");
        Application.Quit();
    }

    // A simple function for buttons that don't load scenes (like your Help button)
    public void PlayClickSound()
    {
        if (clickSound != null)
        {
            clickSound.Play();
        }
    }

    public void OpenInstructions()
    {
        PlayClickSound();
        instructionsPanel.SetActive(true); // Turn the panel on
    }

    public void CloseInstructions()
    {
        PlayClickSound();
        instructionsPanel.SetActive(false); // Turn the panel off
    }
}