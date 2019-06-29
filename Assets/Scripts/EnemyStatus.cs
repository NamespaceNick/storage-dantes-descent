using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour {

    public enum EnemyType { Imp, Harpy, Demon, Boss };
    public EnemyType type;
	public float maxHealth;
	public float dropChance;
	public string weaponName;
	public GameObject drop;

	float health = 0f;
	GameObject weapon;
    GameController gameController;
    AudioBank auBank;

	void Start() {
		health = maxHealth;
		weapon = null;
		if(transform.Find(weaponName) && transform.Find(weaponName).gameObject != gameObject) {
			weapon = transform.Find(weaponName).gameObject;
			weapon.SetActive(false);
		}
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        auBank = GameObject.Find("_AudioBank").GetComponent<AudioBank>();
	}

	void Update()
    {
		if(health <= 0)
        {
			if(Random.value < dropChance) {
				Instantiate(drop, transform.position, Quaternion.identity);
			}
            // Play death sound
            if (auBank.entitySoundsTable.ContainsKey(type) && ((AudioBank.EntitySounds)auBank.entitySoundsTable[type]).death != null) {
                AudioSource.PlayClipAtPoint(((AudioBank.EntitySounds)auBank.entitySoundsTable[type]).death, Camera.main.transform.position + Vector3.forward);
            }
            if (type == EnemyType.Boss)
            {
                gameController.RequestLevelFinish();
            }
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
    {
		if(collider.gameObject.CompareTag("PlayerWeapon")) {
            if (auBank.entitySoundsTable.ContainsKey(type) && ((AudioBank.EntitySounds)auBank.entitySoundsTable[type]).hurt != null) {
                AudioSource.PlayClipAtPoint(((AudioBank.EntitySounds)auBank.entitySoundsTable[type]).hurt, Camera.main.transform.position + Vector3.forward);
            }
			WeaponStats ws = collider.gameObject.GetComponent<WeaponStats>();
			health -= ws.damage;
		}
	}

	public GameObject GetWeapon()
    {
		return weapon;
	}
}
