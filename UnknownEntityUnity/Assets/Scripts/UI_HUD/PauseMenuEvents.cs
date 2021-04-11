using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuEvents : MonoBehaviour
{
    public int mainMenuSceneIndex;
    public MouseInputs moIn;
    public GameObject pauseMenuObject;
    public GameObject optionsMenuObject;
    public TimeSlow timeSlow;

    public void ResumeButton() {
        Time.timeScale = 1f;
        pauseMenuObject.SetActive(false);
        moIn.SwapToPlayerInputs();
        if (timeSlow) timeSlow.UnpauseTimeSlow();
    }

    public void OptionsButton() {
        pauseMenuObject.SetActive(false);
        optionsMenuObject.SetActive(true);

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
