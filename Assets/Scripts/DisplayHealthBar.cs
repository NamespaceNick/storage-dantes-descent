using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayHealthBar : MonoBehaviour {

	public string playerName;

	RectTransform rt;
	float maxWidth = 0;
	float maxHealthInit = 0;

	PlayerStatus ps;

	void Start() {
		rt = GetComponent<RectTransform>();
		maxWidth = rt.sizeDelta.x;
		ps = GameObject.Find(playerName).GetComponent<PlayerStatus>();
		maxHealthInit = ps.maxHealth;
	}

	void Update() {
		rt.sizeDelta = new Vector2(ps.maxHealth / maxHealthInit * maxWidth, rt.sizeDelta.y);
	}
}
