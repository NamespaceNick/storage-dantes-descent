using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

	void OnTriggerEnter(Collider collider) {
		Debug.Log("disable platform");
		Physics.IgnoreCollision(collider, GetComponent<Collider>(), true);
	}

	void OnTriggerExit(Collider collider) {
		Debug.Log("enable platform");
		Physics.IgnoreCollision(collider, GetComponent<Collider>(), false);
	}
}
