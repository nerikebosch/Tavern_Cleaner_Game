using UnityEngine;
using UnityEngine.SceneManagement; // This gives us the power to load scenes!

public class MainMenuController : MonoBehaviour
{
    // Make sure this exactly matches the name of your tavern scene file!
    public string gameSceneName = "Tavern_Scene";

    public void StartGame()
    {
        // This command destroys the Main Menu and loads the Tavern
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        // This will only work in the final built .exe file, not in the editor!
        Debug.Log("Game is Exiting!");
        Application.Quit();
    }
}