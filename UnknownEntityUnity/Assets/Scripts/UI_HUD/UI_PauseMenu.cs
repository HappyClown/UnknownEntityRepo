using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PauseMenu : MonoBehaviour
{
    public MouseInputs moIn;
    public GameObject pauseMenuObject;
    public TimeSlow timeSlow;
    public bool CanIOpenPauseMenu() {
        // What are situations where openning the pause menu while in game would be impossible.
        return true;
    }
    public void OpenPauseMenu() {
        // Maybe pause the game.
        Time.timeScale = 0f;
        // Open the pause menu UI.
        pauseMenuObject.SetActive(true);
        // Swap over to the  
        moIn.SwapToUIInputs();
        if (timeSlow) timeSlow.PauseTimeSlow();
    }
}
