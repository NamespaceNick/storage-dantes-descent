using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// modular script for general main menus and pause menus
public class MenuController : MonoBehaviour
{
    public bool mainMenu = false;
    public bool pauseMenu = false;
    public bool useController = false;
    public bool freezeTime = true;
    public KeyCode pauseKey;
    public GameObject menu;

    bool isPaused = false;
    bool onMainMenu = false;
    Coroutine menuCR;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Assert(mainMenu != pauseMenu);
        Debug.Assert(menu != null);
        menuCR = (mainMenu && (scene.buildIndex == 0)) ? StartCoroutine(HandleMainMenu()) : null;
	}
	
	void Update ()
    {
        if (PauseMenuActivated())
        {
            menuCR = StartCoroutine(HandlePauseMenu());
        }
    }

    bool PauseMenuActivated()
    {
        return Input.GetKeyDown(pauseKey) ? true : false;
    }

    public IEnumerator HandleMainMenu()
    {
        if (onMainMenu) yield break;
        onMainMenu = true;
        Time.timeScale = (freezeTime) ? 0f : Time.timeScale;

        yield break;
    }

    public IEnumerator HandlePauseMenu()
    {
        if (isPaused) yield break;
        isPaused = true;
        Time.timeScale = (freezeTime) ? 0f : Time.timeScale;

        yield break;
    }

    IEnumerator RestartGame(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene(0);
    }

    IEnumerator RestartLevel(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator ExitGame(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Application.Quit();
    }
}
