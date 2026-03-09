using UnityEngine;

public class WhiteHole :HoleBase
{
    protected override void ApplyEffect()
    {
        Collider2D[] objects = FindAffectableObjects();
        foreach (var obj in objects)
        {
            Debug.Log($"Objeto detectado:W {obj.name}");
            if (obj.TryGetComponent(out IHoleAffatable affectable))
            {
                affectable.OnRepelled(transform.position, force);
            }
        }
    }
}
