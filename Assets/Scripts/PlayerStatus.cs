using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatus : MonoBehaviour
{
    public bool causeDeath = false;


    public bool invulnerable = false;
    public bool stunned = false;
    public bool isAlive = true;

    public float maxHealth;
    public float hurtWait;
    public float dashWait;
    public float animWalkSpeedMultiplier;
    public AudioClip playerHurt, playerDeath;
    public AudioClip playerDash, swordSwing;
    public GameObject weapon;
    public GameObject healthBar;

    bool dashing = false;
    bool onAttackCooldown = false;
    bool isAttacking = false;
    bool staticAttacking = false;
    bool beenHurt;
    float currentHealth;
    float originalHealthLength;
    Coroutine attackCoroutine;
    PlayerController controller;
    RespawnPlayer respawn;

    SpriteRenderer sr;
    Animator anim;
    Rigidbody2D rb;

    void Start()
    {
        weapon = GetComponentInChildren<WeaponStats>().gameObject;
        weapon.SetActive(false);
        respawn = GetComponent<RespawnPlayer>();
        currentHealth = maxHealth;
        controller = GetComponent<PlayerController>();
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        originalHealthLength = healthBar.transform.localScale.x;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Entered collision");
    }

    void Update()
    {
        if (causeDeath)
        {
            StartCoroutine(HandleDeath());
        }
        if (dashing || beenHurt)
        {
            sr.color = new Color(1f, 1f, 1f, .5f);
        }
        else
        {
            sr.color = new Color(1f, 1f, 1f, 1f);
        }
        if (!isAttacking)
        {
            if (!controller.GetGrounded())
            {
                anim.speed = 0f;
            }
            else if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                anim.SetBool("idle", true);
                anim.speed = 1f;
            }
            else
            {
                anim.SetBool("idle", false);
                anim.speed = Mathf.Abs(rb.velocity.x) * animWalkSpeedMultiplier;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyWeapon") && !invulnerable && !beenHurt && !dashing)
        {
            PlayerHurt(other.gameObject);
        }
        if (other.CompareTag("HealthPickup"))
        {
            PickupStats pickup = other.GetComponent<PickupStats>();
            currentHealth += pickup.healthUp;
            maxHealth += pickup.maxHealthUp;
            healthBar.transform.localScale = new Vector3(originalHealthLength * (currentHealth / maxHealth), healthBar.transform.localScale.y, healthBar.transform.localScale.z);
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
        // check if collectable
    }


    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("EnemyWeapon") && !invulnerable && !beenHurt && !dashing)
        {
            PlayerHurt(other.gameObject);
        }
        // check if collectable
    }


    public IEnumerator Attack()
    {
        if (!CanAttack())
        {
            yield break;
        }
        AudioSource.PlayClipAtPoint(swordSwing, Camera.main.transform.position);
        onAttackCooldown = true;
        isAttacking = true;
        anim.speed = 1f;
        anim.SetTrigger("swing");
        StartCoroutine(InvulnCooldown());
        weapon.SetActive(true);
        attackCoroutine = StartCoroutine(GetWeaponStats().UseWeapon());
        yield return new WaitForSeconds(GetWeaponStats().animTime);
        isAttacking = false;
        yield return new WaitForSeconds(GetWeaponStats().cooldownTime - GetWeaponStats().animTime);
        onAttackCooldown = false;
    }


    // Checks any status modifiers that restrict attacking
    public bool CanAttack()
    {
        return (onAttackCooldown || invulnerable || beenHurt || dashing) ? false : true;
    }

    // Checks any status modifiers that restrict movement
    public bool CanMove()
    {
        return true;
    }


    public float GetHealth()
    {
        return currentHealth;
    }


    public GameObject GetWeapon()
    {
        return weapon;
    }


    public WeaponStats GetWeaponStats()
    {
        return weapon.GetComponent<WeaponStats>();
    }


    // TODO: This function
    void PickupWeapon()
    {

    }


    void PlayerHurt(GameObject enemyWeap)
    {
        if (invulnerable || !isAlive)
            return;
        stunned = true;
        StartCoroutine(HurtCooldown());
        currentHealth -= enemyWeap.GetComponent<WeaponStats>().damage;
        healthBar.transform.localScale = new Vector3(originalHealthLength * (currentHealth / maxHealth), healthBar.transform.localScale.y, healthBar.transform.localScale.z);
        if (currentHealth <= 0)
        {
            // Player dies, restart level/scene
            StartCoroutine(HandleDeath());
            return;
        }
        else
        {
            AudioSource.PlayClipAtPoint(playerHurt, Camera.main.transform.position + Vector3.forward);
            WeaponStats ws = enemyWeap.GetComponent<WeaponStats>();
            Vector2 kb = new Vector2(ws.knockback.x * enemyWeap.transform.right.x, ws.knockback.y);
            controller.PlayerKnockback(kb);

            // TODO: PlayerHurt audio
        }
        // Check if dead
    }


    IEnumerator HurtCooldown()
    {
        beenHurt = true;
        yield return new WaitForSeconds(hurtWait);
        beenHurt = false;
    }


    IEnumerator InvulnCooldown()
    {
        invulnerable = true;
        yield return new WaitForSeconds(weapon.GetComponent<WeaponStats>().animTime);
        invulnerable = false;
    }

    public IEnumerator DashCooldown()
    {
        dashing = true;
        yield return new WaitForSeconds(dashWait);
        dashing = false;
    }

    // plays death audio clip, calls death function
    IEnumerator HandleDeath()
    {
        if (!isAlive) yield break;
        isAlive = false;
        AudioSource.PlayClipAtPoint(playerDeath, Camera.main.transform.position + Vector3.forward);
        yield return new WaitForSeconds(playerDeath.length);
        Die();
    }

    void Die()
    {
        StopAllCoroutines();
        GameObject.Find("Game Controller").GetComponent<GameController>().RequestRestartGame();
    }
}
