using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonEvents : MonoBehaviour
{
    public int startSceneIndex;
    public GameObject optionsCanvases;
    public GameObject mainMenuCanvases;

    public void StartButton() {
        //Launch game scene, later chracter selection screen
        SceneManager.LoadScene(startSceneIndex);
    }

    public void OptionsButton() {
        // Switch from the main menu to the options menu
        optionsCanvases.SetActive(true);
        mainMenuCanvases.SetActive(false);
    }

    public void ExitButton() {
        // Exit the game
        Application.Quit();
    }

}
