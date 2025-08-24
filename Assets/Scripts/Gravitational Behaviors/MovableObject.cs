using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovableObject : MonoBehaviour, IHoleAffatable
{
    [SerializeField] private FloatSettings floatSetting;
    private Rigidbody2D rb;
    private Vector2 startPosition;
    private float floatOffset;
    private bool isAfected = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        floatOffset = Random.Range(0f, 2f * Mathf.PI);
        if(floatSetting.enableFloatingAnimation) rb.gravityScale = 0f;
    }

    private void FixedUpdate()
    {
        if (floatSetting.enableFloatingAnimation && !isAfected)
        {
            float newY = startPosition.y + Mathf.Sin(Time.time + floatOffset) * floatSetting.floatAmplitude;
            Vector2 newPosition = rb.position;
            newPosition.y = newY;
            rb.position = newPosition;
        }
    }

    public void OnAttracted(Vector2 holePosition, float baseForce)
    {
        isAfected = true;
        Vector2 direction = (Vector2)transform.position - holePosition;
        float distance = direction.magnitude;

        if (distance < floatSetting.minDistanceToHole)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float normalizedDistance = Mathf.Clamp01((distance - floatSetting.minDistanceToHole) / 5f);
        float force = baseForce * floatSetting.maxAttractForceMultiplier * (1f - normalizedDistance);

        Vector2 forceDirection = -direction.normalized;
        rb.AddForce(forceDirection * force, ForceMode2D.Force); 
        isAfected = false;
    }

    public void OnRepelled(Vector2 holePosition, float baseForce)
    {
        isAfected = true;
        Vector2 direction = (Vector2)transform.position - holePosition;
        float distance = direction.magnitude;

        if (distance > floatSetting.maxRepelDistance) return;

        float normalizedDistance = Mathf.Clamp01(distance / floatSetting.maxRepelDistance);
        float force = baseForce * floatSetting.maxRepelForceMultiplier * (1f - normalizedDistance);

        Vector2 forceDirection = direction.normalized;
        rb.AddForce(forceDirection * force, ForceMode2D.Force); 
        isAfected = false;
    }

}
