using UnityEngine;

public class WhiteHole :HoleBase
{
    protected override void ApplyEffect()
    {
        Collider2D[] objects = FindAffectableObjects();
        foreach (var obj in objects)
        {
            if (obj.TryGetComponent(out IHoleAffatable affectable))
            {
                Vector2 direction = (Vector2)obj.transform.position - (Vector2)transform.position;
                affectable.OnRepelled(transform.position, force);
            }
        }
    }
}
