using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{

    Vector3 spawnPoint;

    GameController gameController;
    CameraFollowPlayer camFollow;

	void Start()
    {
        // TODO: Change the respawn point based on where in the level they are
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        camFollow = Camera.main.GetComponent<CameraFollowPlayer>();
		spawnPoint = transform.position;
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Game Bounds"))
        {
            Respawn();
        }
    }

    // drops the player at their nearest passed checkpoint
    public void Respawn()
    {
        transform.position = gameController.GetSpawnLocation();

        // FIXME: newLevel is a bit hacky, there's a better way to take care of this
        if (gameController.GetComponent<GameController>().newLevel)
        {
            gameController.GetComponent<GameController>().newLevel = false;
            camFollow.transform.position = gameController.GetComponent<GameController>().currentCameraStart;
            camFollow.moveY = false;
        }
        else
        {
            camFollow.moveY = true;
        }
    }
}
