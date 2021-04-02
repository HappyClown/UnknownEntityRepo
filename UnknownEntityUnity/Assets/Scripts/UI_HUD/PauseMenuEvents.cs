using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuEvents : MonoBehaviour
{
    public int mainMenuSceneIndex;
    public MouseInputs moIn;
    public GameObject pauseMenuObject;

    public void ResumeButton() {
        pauseMenuObject.SetActive(false);
        moIn.SwapToPlayerInputs();
    }

    public void BackToMainMenu() {
        // Go back to main menu.
        SceneManager.LoadScene(mainMenuSceneIndex);
    }

    public void ExitGameButton() {
        // Exit the game
        Application.Quit();
    }
}
