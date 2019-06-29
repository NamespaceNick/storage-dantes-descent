using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{


    bool beenReached = false;
    GameController gameController;
	// Use this for initialization
	void Start ()
    {
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    void Reached()
    {
        beenReached = true;
    }
}
