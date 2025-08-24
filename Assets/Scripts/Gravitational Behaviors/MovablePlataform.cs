using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FloatingHeavyPlatform : MonoBehaviour, IHoleAffatable
{
    [Header("Configuraci�n de Flotacion")]
    [SerializeField] private FloatSettings floatSetting;
    [SerializeField] private bool moveOnlyOnX = true;

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private float floatOffset;
    private bool isAffected = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        floatOffset = Random.Range(0f, 2f * Mathf.PI);
        if (floatSetting.enableFloatingAnimation) rb.gravityScale = 0f;
    }

    private void FixedUpdate()
    {
        if (floatSetting.enableFloatingAnimation && !isAffected)
        {
            Vector2 currentPosition = rb.position;
            float newY = startPosition.y + Mathf.Sin(Time.time + floatOffset) * floatSetting.floatAmplitude;
            Vector2 newPosition = currentPosition;
            newPosition.y = newY;
            rb.position = newPosition;
        }
    }

    public void OnAttracted(Vector2 holePosition, float baseForce)
    {
        isAffected = true;

        Vector2 direction = (Vector2)transform.position - holePosition;
        float distance = direction.magnitude;

        if (distance < floatSetting.minDistanceToHole)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float normalizedDistance = Mathf.Clamp01((distance - floatSetting.minDistanceToHole) / 5f);
        float forceMagnitude = baseForce * floatSetting.maxAttractForceMultiplier * (1f - normalizedDistance);
        Vector2 forceDirection = -direction.normalized;

        if (moveOnlyOnX) forceDirection.y = 0;
        else forceDirection.x = 0;

        rb.AddForce(forceDirection * forceMagnitude, ForceMode2D.Force);
        isAffected = false;
    }

    public void OnRepelled(Vector2 holePosition, float baseForce)
    {
        isAffected = true;

        Vector2 direction = (Vector2)transform.position - holePosition;
        float distance = direction.magnitude;

        if (distance > floatSetting.maxRepelDistance) return;

        float normalizedDistance = Mathf.Clamp01(distance / floatSetting.maxRepelDistance);
        float forceMagnitude = baseForce * floatSetting.maxRepelForceMultiplier * (1f - normalizedDistance);
        Vector2 forceDirection = direction.normalized;

        if (moveOnlyOnX) forceDirection.y = 0;
        else forceDirection.x = 0;

        rb.AddForce(forceDirection * forceMagnitude, ForceMode2D.Force);
        isAffected = false;
    }
}

