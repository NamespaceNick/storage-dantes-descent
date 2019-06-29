using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupStats : MonoBehaviour {

	public float healthUp;
	public float maxHealthUp;

	void OnTriggerEnter2D(Collider2D collider) {
		if(collider.gameObject.CompareTag("Player")) {
			Destroy(gameObject);
		}
	}
}
