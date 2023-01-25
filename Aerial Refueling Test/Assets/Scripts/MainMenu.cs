using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string startGameScene;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        SceneManager.LoadScene(startGameScene);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }

    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void Controls()
    {
        SceneManager.LoadScene("Joystick");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void BackToControls()
    {
        SceneManager.LoadScene("Controls");
    }

    public void ToJoyStick()
    {
        SceneManager.LoadScene("JoyStick");
    }

    public void ToKeyboard()
    {
        SceneManager.LoadScene("Keyboard");
    }

    public void ToController()
    {
        SceneManager.LoadScene("Controller");
    }
}
