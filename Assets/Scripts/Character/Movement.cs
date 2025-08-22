using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour 
{
    [Header("Componentes")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PhysicsManager physics;

    [Header("Movement Parameters")]
    [SerializeField] private float airMultiplier;
    [SerializeField] private float airFriction;
    [SerializeField] private float groundFriction;

    [Header("Movement Variables")]
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;

    [Header("Input")]
    [SerializeField] private Vector2 input;
    private bool isFacingRight = true;

    // Propiedad para identificar si el personaje esta volteando
    public bool isTurning => (rb.linearVelocity.x > 0 && input.x < 0) || (rb.linearVelocity.x < 0 && input.x > 0);

    private void FixedUpdate()
    {
        Move();
        Friction();
        LimitSpeed();
        FlipCharacter();
    }

    public void GetInput(InputAction.CallbackContext contex)
    {
        input = contex.ReadValue<Vector2>();
    }

    private void Move()
    {
        Vector2 direction = Vector2.right * input;

        float currentAceleration = physics.OnGround ? acceleration : acceleration * airMultiplier;
        rb.AddForce(direction * currentAceleration, ForceMode2D.Force); 
    }

    private void Friction()
    {
        if (input.x == 0 || isTurning)
        {
            float frictionFactor = physics.OnGround ? groundFriction : airFriction;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x * frictionFactor, rb.linearVelocity.y);
        }
    }

    private void LimitSpeed()
    {
        if (Mathf.Abs(rb.linearVelocity.x) > maxSpeed)
        {
            float limitedSpeed = Mathf.Sign(rb.linearVelocity.x) * maxSpeed;
            rb.linearVelocity = new Vector2(limitedSpeed, rb.linearVelocity.y);
        }
    }


    private void FlipCharacter()
    {
        if ((input.x > 0 && !isFacingRight) || (input.x < 0 && isFacingRight))
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    public void SetMaxSpeed(float maxSpeed) => this.maxSpeed = maxSpeed;
    public void SetAcceleration(float acceleration) => this.acceleration = acceleration;   

}