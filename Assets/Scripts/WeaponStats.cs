using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    public bool requiresStop = false;
    public float damage;
    // animTime <= cooldownTime
    public float animTime;
    public float cooldownTime;
    public Vector3 playerOffset;
    public Vector3 knockback;
    public Sprite spr;
    /* Re-enable once we have multiple sprites that need to play sequentially
    public Sprite[] spriteArray;
    public float[] animTimes;
    */

    // TODO: This needs to be changed once we import the sprites
    // SpriteRenderer rend;
    GameObject player;
    // Use this for initialization
    void Awake()
    {
        player = GameObject.Find("Player");
    }


    void Start ()
    {
	}

    private void OnTriggerEnter(Collider other)
    {

    }

    public IEnumerator UseWeapon()
    {
        yield return new WaitForSeconds(animTime);
        Debug.Log("Finished attacking");
        gameObject.SetActive(false);
    }
}
