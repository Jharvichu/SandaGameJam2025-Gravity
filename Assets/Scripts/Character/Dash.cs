using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Dash : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Movement move; 
    [SerializeField] private PhysicsManager physics;

    [Header("Dash Parameters")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float coyoteDashTime;

    [Header("Dash Time")]
    [SerializeField] private float dashCooldownTimer;
    [SerializeField] private float dashTimeCounter;
    [SerializeField] private bool isDashing;
    public bool IsDashing => isDashing;
    private float baseGravity;

    [Header("Animacion")]
    [SerializeField] private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (dashCooldownTimer > 0) dashCooldownTimer -= Time.deltaTime;
    }

    public void GetInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if (dashCooldownTimer <= 0 && Time.time - physics.LastGroundTime >= coyoteDashTime)
            {
                animator.SetTrigger("Dash");
                baseGravity = rb.gravityScale;
                int direction = move.IsFacingRight ? 1 : -1;
                StartCoroutine(DashAction(direction));
            }
        }
    }

    private IEnumerator DashAction(int direction)
    {
        rb.gravityScale = 0f;
        isDashing = true;

        while(dashTimeCounter <= dashDuration)
        {
            rb.linearVelocity = new Vector2(direction * dashSpeed, 0);
            dashTimeCounter += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
        rb.gravityScale = baseGravity;
        dashTimeCounter = 0;
        dashCooldownTimer = dashCooldown;
    }

}