using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public GameObject gameStart;
    public Vector3 currentCameraStart;
    public bool newLevel = false;

    GameObject currentSpawnPoint;
    GameObject player;
    AudioBank auBank;


	void Start ()
    {
        auBank = GameObject.Find("_AudioBank").GetComponent<AudioBank>();
        currentCameraStart = Camera.main.transform.position;
        player = GameObject.Find("Player");
        currentSpawnPoint = gameStart;
	}

    // When player falls down a pit, placed back at the beginning of 
    // their current circle, possibly respawn enemies
    void RestartLevel()
    {
    }


    public Vector3 GetSpawnLocation()
    {
        return currentSpawnPoint.transform.position;
    }


    // Updates the current spawnpoint to be the point associated
    // with the most recently passed checkpoint
    public void CheckpointReached(GameObject newSpawnPoint)
    {
        currentSpawnPoint = newSpawnPoint;
        // TODO: Cause camera to move to new position
    }

    // When the player dies, it will invoke this function, to respawn
    // all enemies and begin the player back at the first circle
    public void RequestRestartGame()
    {
        // TODO: Some if to make sure this isn't called multiple times
        StartCoroutine(RestartGame());
    }

    IEnumerator RestartGame()
    {
        yield return null;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // when the level has been beaten, play finish audioclip, then move on to next scene
    public void RequestLevelFinish()
    {
        StartCoroutine(LevelFinish());
    }

    IEnumerator LevelFinish()
    {
        yield return new WaitForSeconds(11f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
