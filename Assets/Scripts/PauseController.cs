using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PauseController : MonoBehaviour {

    public GameObject pausePanel;

    string state = "Active";
    InputDevice device;

    private void Update() {
        device = InputManager.ActiveDevice;
        if ((Input.GetKeyDown(KeyCode.Escape) || device.Command.WasPressed) && state == "Active") {
            pausePanel.SetActive(true);
            Time.timeScale = 0f; //this pauses the game
            state = "Paused";
        }
        else if ((Input.GetKeyDown(KeyCode.Escape) || device.Command.WasPressed) && state == "Paused") {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
            state = "Active";
        }
    }

}
