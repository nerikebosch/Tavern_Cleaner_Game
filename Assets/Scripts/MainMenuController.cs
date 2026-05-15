using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    public string gameSceneName = "Tavern_Scene";

    [Header("Audio")]
    public AudioSource clickSound; // The sound we want to play
    public float delayBeforeLoad = 0.3f; // How long to wait before loading the scene

    [Header("UI Panels")]
    public GameObject instructionsPanel;

    public void StartGame()
    {
        PlayClickSound();

        // Start the timer to delay the scene load
        StartCoroutine(LoadSceneWithDelay());
    }

    IEnumerator LoadSceneWithDelay()
    {
        yield return new WaitForSeconds(delayBeforeLoad);

        // Load the Tavern!
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

    // A simple function for buttons that don't load scenes
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