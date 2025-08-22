using UnityEngine;
public abstract class HoleBase : MonoBehaviour
{
    [SerializeField] protected float radius = 5f;
    [SerializeField] protected float force = 10f;
    [SerializeField] protected float lifetime = 3f;
    [SerializeField] protected LayerMask affectableLayers;

    protected virtual void Start()
    {
        Destroy(gameObject, lifetime);
    }

    protected virtual void Update()
    {
        ApplyEffect();
    }

    protected abstract void ApplyEffect();

    protected Collider2D[] FindAffectableObjects()
    {
        return Physics2D.OverlapCircleAll(transform.position, radius, affectableLayers);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y, 0), radius);
    }
}

