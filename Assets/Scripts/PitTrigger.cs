using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitTrigger : MonoBehaviour {

    public bool isEntrance = false;
    CameraFollowPlayer camFollow;

	void Start () {
        camFollow = Camera.main.GetComponent<CameraFollowPlayer>();
	}


    void OnTriggerEnter2D(Collider2D collision)
    {
        camFollow.moveY = isEntrance ? true : false;
    }
}
