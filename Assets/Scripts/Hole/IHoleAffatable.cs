using UnityEngine;

public interface IHoleAffatable
{
    void OnAttracted(Vector2 holePosition, float force);
    void OnRepelled(Vector2 holePosition, float force);
}
