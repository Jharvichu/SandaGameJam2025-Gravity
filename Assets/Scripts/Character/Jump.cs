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
    [SerializeField] private float movementBonus;

    [Header("Jump Time")]
    [SerializeField] private bool jumpAction;
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
    }

    public void getInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (jumpCooldownTimer <= 0 && Time.time - physics.LastGroundTime <= coyoteJumpTime)
            {
                JumpAction();
                remainingJumps = extraJumps;
            }
            else if (jumpCooldownTimer <= 0 && Time.time - physics.LastGroundTime >= coyoteJumpTime && remainingJumps > 0)
            {
                JumpAction();
                remainingJumps -= 1;
            }
        }
    }

    private void JumpAction()
    {
        float speed = rb.linearVelocity.magnitude;
        float bonus = movementBonus * speed;

        rb.linearVelocity = new Vector3(rb.linearVelocityX, 0);
        rb.AddForce(Vector2.up * (jumpForce * planetGravitySettings.JumpForceMultiplier + bonus), ForceMode2D.Impulse);

        jumpCooldownTimer = jumpCooldown;
    }

    private void SetGravityUse()
    {
        Physics2D.gravity = new Vector3(0, planetGravitySettings.Gravity, 0);
    }


}
