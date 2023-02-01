using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// The name of the scene to load when the player starts the game.
    /// </summary>
    public string startGameScene;

    /// <summary>
    /// Starts the game by loading the scene specified in startGameScene.
    /// (old way of calling, now scene to load is decided on the loading screen itself)
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene(startGameScene);
    }

    /// <summary>
    /// Restarts the game by loading the scene.
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    /// <summary>
    /// Exits the game. (only works in build, not in the editor)
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Loads the settings menu.
    /// </summary>
    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }

    /// <summary>
    /// Loads the controls menu (starting on the joystick scene).
    /// </summary>
    public void Controls()
    {
        SceneManager.LoadScene("Joystick");
    }

    /// <summary>
    /// Loads the main menu.
    /// </summary>
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Loads the controls menu
    /// </summary>
    public void BackToControls()
    {
        SceneManager.LoadScene("Controls");
    }

    /// <summary>
    /// Loads the joystick controls page
    /// </summary>
    public void ToJoyStick()
    {
        SceneManager.LoadScene("JoyStick");
    }

    /// <summary>
    /// Loads the keyboard controls page
    /// </summary>
    public void ToKeyboard()
    {
        SceneManager.LoadScene("Keyboard");
    }

    /// <summary>
    /// Loads the controller controls page
    /// </summary>
    public void ToController()
    {
        SceneManager.LoadScene("Controller");
    }
}
