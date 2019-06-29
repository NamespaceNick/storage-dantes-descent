using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PlayerController : MonoBehaviour
{
    public float speed, speedLerp;
    public float jumpSpeed, jumpLerp;
    public float gravityMultiplier;
    public float dashSpeed, dashAddition;
    public float dashMin, dashMax, dashCooldownTime;
    public float slowMultiplier;
    public float dashVelInvuln;
    public float fullDashSpeed;
    // public AudioClip[] steps = new AudioClip[4];




    bool chargingDash = false;
    bool onDashCooldown = false;
    bool isGrounded = false;
    bool knockedBack = false;
    float hDir, vDir;

    AudioBank auBank;
    Coroutine attackCoroutine;
    Coroutine dashDetectCR, dashCooldownCR;
    Coroutine stepsCR;
    InputDevice device;
    // TODO: use incontrol player actions
    // PlayerAction left, right, jump, attack;
    Rigidbody2D rb;
    PlayerStatus status;
    Vector2 facing;

	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        status = GetComponent<PlayerStatus>();
	}


    void Update()
    {
        device = InputManager.ActiveDevice;
        if (status.isAlive)
            HandleInput();
        else
        {
            hDir = 0;
            rb.velocity = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        float hSpeed;
        if (isGrounded || Mathf.Abs(rb.velocity.x) > speed)
        {
            hSpeed = Mathf.Lerp(rb.velocity.x, hDir * speed, speedLerp);
        }
        else
        {
            hSpeed = Mathf.Lerp(rb.velocity.x, hDir * speed, jumpLerp);

        }
        if (chargingDash && isGrounded)
        {
            hSpeed *= slowMultiplier;
        }
        rb.velocity = new Vector2(hSpeed, rb.velocity.y);
        if(hDir != 0) {
          transform.right = hDir * Vector2.right;
        }
      }


    void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO: Handle this in a more sophisticated way, probably raycasts
        // set grounded
        if (collision.gameObject.CompareTag("Ground") && (collision.contacts[0].normal == Vector2.up))
            isGrounded = true;
    }


    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && (collision.contacts[0].normal == Vector2.up))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }


    // Handles movement and jump inputs
    void HandleInput()
    {
        hDir = 0f;
        vDir = 0f;
        // TODO: Account for left and right being pressed on keyboard
        if (Input.GetKey(KeyCode.A) || device.LeftStickLeft.IsPressed)
        {
            hDir = -1f;
            facing = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.D) || device.LeftStickRight.IsPressed)
        {
            hDir = 1f;
            facing = Vector2.right;
        }
        if ((Input.GetKeyDown(KeyCode.Space) || device.Action1.WasPressed) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
        if ((Input.GetKeyDown(KeyCode.Return) || device.Action3.WasPressed) && status.CanAttack())
        {
            attackCoroutine = StartCoroutine(status.Attack());
        }
        if ((Input.GetKeyDown(KeyCode.RightShift) || device.Action2.WasPressed))
        {
            dashDetectCR = StartCoroutine(DetectDash());
        }
    }


    bool CanJump()
    {
        return (isGrounded && !chargingDash && !knockedBack) ? true : false;
    }


    IEnumerator DetectDash()
    {
        if (onDashCooldown || status.invulnerable)
        {
            yield break;
        }
        float origSpeed = speed;
        dashSpeed = dashMin;
        // while charging dash
        while (Input.GetKey(KeyCode.RightShift) || device.Action2.IsPressed)
        {
            chargingDash = true;
            dashSpeed += dashAddition * Time.deltaTime;
            dashSpeed = Mathf.Min(dashSpeed, dashMax);
            yield return null;
        }
        // dash has begun
        AudioSource.PlayClipAtPoint(status.playerDash, Camera.main.transform.position + Vector3.forward);
        chargingDash = false;
        // status.invulnerable = true;
        // rb.velocity = new Vector3(dashSpeed * facing, rb.velocity.y, rb.velocity.z);
        float vSpeed = rb.velocity.y;
        if(dashSpeed > fullDashSpeed) {
          vSpeed = 0;
          StartCoroutine(status.DashCooldown());
        }
        rb.velocity = new Vector2(facing.x * dashSpeed, vSpeed);
        dashSpeed = dashMin;
        dashCooldownCR = StartCoroutine(DashCooldown());
    }


    public void PlayerKnockback(Vector2 knockback)
    {
        rb.velocity = knockback;
    }


    IEnumerator DashCooldown()
    {
        onDashCooldown = true;
        yield return new WaitForSeconds(dashCooldownTime);
        onDashCooldown = false;
    }

    /*
    // TODO: This function
    IEnumerator StepsAudio()
    {
    }
    */

    public bool GetGrounded() {
      return isGrounded;
    }
}
