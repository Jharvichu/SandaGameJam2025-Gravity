using UnityEngine;

public class BlackHole : HoleBase
{
    protected override void ApplyEffect()
    {
        Collider2D[] objects = FindAffectableObjects();
        foreach (var obj in objects)
        {
            Debug.Log($"Objeto detectado N: {obj.name}");
            if (obj.TryGetComponent(out IHoleAffatable affectable))
            {
                Debug.Log($"Objeto detectado afectado N: {obj.name}");
                affectable.OnAttracted(transform.position, force);
            }
        }
    }
}
