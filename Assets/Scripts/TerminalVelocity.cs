using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalVelocity : MonoBehaviour {

	public float terminalVelocity = 20f;

	Rigidbody2D rb;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
	}

	void Update() {
		if(rb.velocity.y < -terminalVelocity) {
			rb.velocity = new Vector2(rb.velocity.x, -terminalVelocity);
		}
	}
}
