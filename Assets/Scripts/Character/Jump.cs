using UnityEngine;
using UnityEngine.InputSystem;

public class Jump : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PhysicsManager physics;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float extraJumps;
    [SerializeField] private float coyoteJumpTime;

    [Header("Variable Jump")]
    [SerializeField] private float maxJumpTime;
    [SerializeField] private float jumpTimeCounter;
    [SerializeField] private bool isJumping;

    [Header("Jump Time")]
    [SerializeField] private float remainingJumps;
    [SerializeField] private float jumpCooldownTimer;

    [Header("Planet Gravity")]
    [SerializeField] private GravitySettings planetGravitySettings;

    private void Update()
    {
        if (jumpCooldownTimer > 0) jumpCooldownTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        SetGravityUse();

        if (isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce * planetGravitySettings.JumpForceMultiplier * 0.75f);
                jumpTimeCounter -= Time.fixedDeltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
    }

    public void getInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (jumpCooldownTimer <= 0 && Time.time - physics.LastGroundTime <= coyoteJumpTime)
            {
                StartJump();
                remainingJumps = extraJumps;
            }
            else if (jumpCooldownTimer <= 0 && Time.time - physics.LastGroundTime >= coyoteJumpTime && remainingJumps > 0)
            {
                StartJump();
                remainingJumps -= 1;
            }
        }
        else if (context.canceled)
        {
            isJumping = false;
            jumpTimeCounter = 0;
        }
    }

    private void StartJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce * planetGravitySettings.JumpForceMultiplier);
        
        jumpCooldownTimer = jumpCooldown;
        isJumping = true;
        jumpTimeCounter = maxJumpTime;
    }

    private void SetGravityUse()
    {
        Physics2D.gravity = new Vector3(0, planetGravitySettings.Gravity, 0);
    }

}
