using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsButtonEvents : MonoBehaviour
{
    public GameObject optionsCanvases;
    public GameObject mainMenuCanvases;

    public void BackButton() {
        // Go from the options menu back to the main menu
        optionsCanvases.SetActive(false);
        mainMenuCanvases.SetActive(true);
    }
}
