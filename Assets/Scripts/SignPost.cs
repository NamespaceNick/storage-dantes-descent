using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignPost : MonoBehaviour
{
    public bool isCheckpoint = false;
    public GameObject spawnPoint;
    // Where the camera jumps to for next level, if this sign is a checkpoint
    public GameObject camStartPoint; 


    GameController gameController;
    RespawnPlayer respawnPlayer;


    void Start ()
    {
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        respawnPlayer = GameObject.Find("Player").GetComponent<RespawnPlayer>();
        if (isCheckpoint && ((spawnPoint == null) || camStartPoint == null))
        {
            Debug.LogError("Checkpoint or start point at " + transform.position + " does not have a spawn point assigned to it.");
        }
	}


    void OnTriggerEnter2D(Collider2D collision)
    {
        // Make text box appear
        // TODO: Possibly freeze player before removing floor below them
        if (collision.CompareTag("Player"))
        {
            if (isCheckpoint)
            {
                // Maybe a coroutine that does this so the player can read it
                // Freeze player, remove floor, indicate spawnPoint
                gameController.CheckpointReached(spawnPoint);
                gameController.currentCameraStart = camStartPoint.transform.position;
                gameController.newLevel = true;
                // TODO: Remove the floor under the player
            }
            // TODO: Change to not an else if we decide to let the player 
            // read the sign
            else
            {
                // TODO: Make the text appear
            }


        }
        
    }
	void Update ()
    {
		
	}
}
