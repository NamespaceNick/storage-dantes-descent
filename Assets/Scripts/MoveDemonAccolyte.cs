using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDemonAccolyte : MonoBehaviour
{

    public float speed, speedLerp;
    public float jumpSpeed, jumpLerp;
    public float minJumpWait, maxJumpWait;
    public float sightRange, sightMax;
    public float swingDelay;
    public float hRange, vRange;
    public float knockbackMultiplier;
    public float restDelay;
    public float rayCastDist;
    public float animWalkSpeedMultiplier;
    public Vector3 swingOffset;
    public string playerName;

    bool sighted = false, isGrounded = false, facingWall = false;
    bool canJump = true, canSwing = true, canMove = true;
    float hDir = 0;
    GameObject player;
    EnemyStatus es;
    Rigidbody2D rb;
    Animator anim;

    void Start()
    {
        player = GameObject.Find(playerName);
        es = GetComponent<EnemyStatus>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (anim != null)
        {
            if (!isGrounded)
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

        if ((player.transform.position - transform.position).magnitude < sightRange)
        {
            sighted = true;
        }
        if ((player.transform.position - transform.position).magnitude > sightMax)
        {
            sighted = false;
        }


        if (sighted)
        {
            if (canMove && !(Mathf.Abs(player.transform.position.x - transform.position.x) < hRange && Mathf.Abs(player.transform.position.y - transform.position.y) < vRange))
            {
                hDir = (player.transform.position.x - transform.position.x) / Mathf.Abs(player.transform.position.x - transform.position.x);
            }
            else
            {
                hDir = 0f;
            }
            StartCoroutine(RandomJump());
            if (canSwing && Mathf.Abs(player.transform.position.x - transform.position.x) < hRange && Mathf.Abs(player.transform.position.y - transform.position.y) < vRange)
            {
                StartCoroutine(Swing());
            }
        }
    }

    void FixedUpdate()
    {
        float hSpeed = Mathf.Lerp(rb.velocity.x, hDir * speed, speedLerp);
        rb.velocity = new Vector2(hSpeed, rb.velocity.y);
        if (Mathf.Abs(rb.velocity.x) > .1f)
        {
            transform.right = new Vector2(rb.velocity.x, 0f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && (collision.contacts[0].normal == Vector2.up))
        {
            isGrounded = true;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && (collision.contacts[0].normal == Vector2.up))
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Ground") && (collision.contacts[0].normal.x != 0))
        {
            facingWall = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            facingWall = false;
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

    IEnumerator RandomJump()
    {
        if (!canJump)
        {
            yield break;
        }
        canJump = false;
        if (isGrounded && (player.transform.position.y > transform.position.y ||
            !Physics2D.Raycast(transform.position, Vector2.down, rayCastDist) ||
            Physics2D.Raycast(transform.position, transform.right, rayCastDist,
            LayerMask.NameToLayer("Ground"))))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
        yield return new WaitForSeconds(Random.Range(minJumpWait, maxJumpWait));
        canJump = true;
    }

    IEnumerator Swing()
    {
        GameObject weapon = es.GetWeapon();
        if (weapon != null)
        {
            float swingDir = hDir;
            canSwing = false;
            canMove = false;
            yield return new WaitForSeconds(swingDelay);
            WeaponStats ws = weapon.GetComponent<WeaponStats>();
            weapon.SetActive(true);
            StartCoroutine(ws.UseWeapon());
            yield return new WaitForSeconds(restDelay);
            canMove = true;
            yield return new WaitForSeconds(ws.cooldownTime - restDelay);
            canSwing = true;
        }
    }
}
