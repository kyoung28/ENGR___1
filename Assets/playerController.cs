using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    private Vector2 movement = Vector2.zero;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;
    private Vector3 dashDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
        Debug.Log(movement);
    }

    void OnDash()
    {
        if (!isDashing && dashCooldownTimer <= 0f && movement != Vector2.zero)
        {
            isDashing = true;
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;

            // Normalize to prevent faster diagonal dashes
            dashDirection = new Vector3(movement.x, 0, movement.y).normalized;
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            rb.velocity = dashDirection * dashSpeed;
            dashTimer -= Time.fixedDeltaTime;
            if (dashTimer <= 0f)
            {
                isDashing = false;
            }
        }
        else
        {
            Move(movement.x, movement.y);
        }

        // Countdown for cooldown
        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.fixedDeltaTime;
        }
    }

    private void Move(float x, float z)
    {
        rb.velocity = new Vector3(x * speed, rb.velocity.y, z * speed);
    }
}
