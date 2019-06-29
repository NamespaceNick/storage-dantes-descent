using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHarpy : MonoBehaviour {

	public float sightRange, sightMax;
	public float moveSpeed;
	public float swoopMultiplier;
	public float attackOffset, xOffset, yOffset;
	public string playerName;
	public float lerp;
	public float knockbackMultiplier;

	bool sighted = false;
	bool started = false;
	float swoopDir;
	Vector2 targetVel;
	GameObject player;
	Rigidbody2D rb;

	void Start() {
		swoopDir = -1f;
		player = GameObject.Find(playerName);
		rb = GetComponent<Rigidbody2D>();
	}

	void Update() {
		if ((player.transform.position - transform.position).magnitude < sightRange)
		{
				sighted = true;
		}
		if ((player.transform.position - transform.position).magnitude > sightMax)
		{
				sighted = false;
		}

		if(sighted) {
			if(transform.position.y - player.transform.position.y > yOffset) {
				swoopDir = (player.transform.position.x - transform.position.x) / Mathf.Abs(player.transform.position.x - transform.position.x);
			}
			if(swoopDir * (transform.position.x - player.transform.position.x) > xOffset && transform.position.y - player.transform.position.y < yOffset) {
				targetVel = Vector2.up * moveSpeed;
			}
			else if(transform.position.y - player.transform.position.y > attackOffset) {
				targetVel = Vector2.down * moveSpeed;
			}
			else {
				targetVel = Vector2.right * swoopDir * moveSpeed;
			}
		}
		else {
			targetVel = Vector2.zero;
		}
	}

	void FixedUpdate() {
		rb.velocity = Vector2.Lerp(rb.velocity, targetVel, lerp);
		if(Mathf.Abs(rb.velocity.x) > .1f) {
			transform.right = new Vector2(rb.velocity.x, 0f);
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
			if (collider.gameObject.CompareTag("PlayerWeapon"))
			{
					Vector2 dir = collider.gameObject.transform.right;
					WeaponStats ws = collider.gameObject.GetComponent<WeaponStats>();
					rb.velocity = knockbackMultiplier * (new Vector2(ws.knockback.x * dir.x, ws.knockback.y));
			}
	}
}
