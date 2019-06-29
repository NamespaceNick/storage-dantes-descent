using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour {

    public bool moveY = false;
	public float maxHOffset, maxVOffset;
	public float speed;
	public float lerp;
	public string playerName;

	GameObject player;

	void Start() {
		player = GameObject.Find(playerName);
		transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
	}

	void LateUpdate()  {
		float hPos = transform.position.x;
		float dir = 0f;
		if(player.transform.position.x - maxHOffset > transform.position.x) {
			hPos = player.transform.position.x - maxHOffset;
			dir = 1f;
		}
		else if(player.transform.position.x + maxHOffset < transform.position.x) {
			hPos = player.transform.position.x + maxHOffset;
			dir = -1f;
		}
		float lerpDist = Mathf.Lerp(transform.position.x, hPos, lerp) - transform.position.x;
		float speedDist = dir * speed * Time.deltaTime;
		if(Mathf.Abs(hPos - transform.position.x) < Mathf.Abs(speedDist))
		{
			transform.position = new Vector3(hPos, transform.position.y, transform.position.z);
		}
		else if (Mathf.Abs(lerpDist) > Mathf.Abs(speedDist)) {
			transform.position = new Vector3(transform.position.x + lerpDist, transform.position.y, transform.position.z);
		}
		else {
			transform.position = new Vector3(transform.position.x + speedDist, transform.position.y, transform.position.z);
		}
    if(moveY) {
      float vPos = transform.position.y;
      dir = 0f;
      if(player.transform.position.y - maxVOffset > transform.position.y) {
        vPos = player.transform.position.y - maxHOffset;
        dir = 1f;
      }
      else if(player.transform.position.y + maxVOffset < transform.position.y) {
        vPos = player.transform.position.y + maxVOffset;
        dir = -1f;
      }
      lerpDist = Mathf.Lerp(transform.position.y, vPos, lerp) - transform.position.y;
      speedDist = dir * speed * Time.deltaTime;
      if(Mathf.Abs(vPos - transform.position.y) < Mathf.Abs(speedDist))
      {
        transform.position = new Vector3(transform.position.x, vPos, transform.position.z);
      }
      else if (Mathf.Abs(lerpDist) > Mathf.Abs(speedDist)) {
        transform.position = new Vector3(transform.position.x, transform.position.y + lerpDist, transform.position.z);
      }
      else {
        transform.position = new Vector3(transform.position.x, transform.position.y + speedDist, transform.position.z);
      }
    }
	}
}
