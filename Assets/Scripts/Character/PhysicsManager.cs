using UnityEngine;

public class PhysicsManager : MonoBehaviour
{
    [Header("Physics Component")]
    [SerializeField] private Transform player;

    [Header("Ground")]
    [SerializeField] private Vector3 groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private bool onGround;
    public bool OnGround => onGround;
    [SerializeField] private RaycastHit2D groundHit;
    public RaycastHit2D GroundHit => groundHit;
    [SerializeField] private float lastGroundTime;
    public float LastGroundTime => lastGroundTime;

    private void Update()
    {
        UpdateLastGroundTime();
    }

    private void FixedUpdate() 
    {
        GroundCheck();    
    }

    private void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(player.position + groundCheck, Vector2.up * -1, groundCheckDistance, whatIsGround);

        onGround = hit.collider != null;

        if (hit.collider != null) groundHit = hit;
    }

    private void UpdateLastGroundTime()
    {
        if(!onGround) return;

        lastGroundTime = Time.time;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(player.position + groundCheck, Vector3.up * -1 * groundCheckDistance);
    }
}