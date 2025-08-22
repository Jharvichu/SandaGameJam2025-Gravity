using UnityEngine;

public class BlackHole : HoleBase
{
    protected override void ApplyEffect()
    {
        Collider2D[] objects = FindAffectableObjects();
        foreach (var obj in objects)
        {
            if (obj.TryGetComponent(out IHoleAffatable affectable))
            {
                Vector2 direction = (Vector2)transform.position - (Vector2)obj.transform.position;
                affectable.OnAttracted(transform.position, force);
            }
        }
    }
}
