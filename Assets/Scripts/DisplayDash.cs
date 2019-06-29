using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayDash : MonoBehaviour {

	public float flashDuration;
	public string playerName;

	RectTransform rt;
	Image image;
	PlayerController pc;
	float minHeight;
	bool canFlash = true;

	void Start() {
		rt = GetComponent<RectTransform>();
		image = GetComponent<Image>();
		pc = GameObject.Find(playerName).GetComponent<PlayerController>();
		minHeight = rt.offsetMax.y;
	}

	void Update() {
		float dashPercent = (pc.dashSpeed - pc.dashMin) / (pc.dashMax - pc.dashMin);
		rt.offsetMax = new Vector2(rt.offsetMax.x, (1f-dashPercent) * minHeight);
		float fullPercent = (pc.fullDashSpeed - pc.dashMin) / (pc.dashMax - pc.dashMin);
		if(dashPercent < fullPercent) {
			image.color = Color.cyan;
		}
		else if (dashPercent < 1f) {
			image.color = Color.yellow;
		}
		else {
			StartCoroutine(Flash());
		}
	}

	IEnumerator Flash() {
		if(!canFlash) {
			yield break;
		}
		canFlash = false;
		image.color = Color.cyan;
		yield return new WaitForSeconds(flashDuration);
		image.color = Color.yellow;
		yield return new WaitForSeconds(flashDuration);
		canFlash = true;
	}
}
