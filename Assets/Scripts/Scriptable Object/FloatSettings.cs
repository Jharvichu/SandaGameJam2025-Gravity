using UnityEngine;

[CreateAssetMenu(fileName = "NewFloatSettings", menuName = "Scriptable Objects/FloatSettings")]
public class FloatSettings : ScriptableObject
{
    [Header("Flotacion")]
    public bool enableFloatingAnimation;
    public float floatAmplitude;

    [Header("Atraccion")]
    public float minDistanceToHole;
    public float maxAttractForceMultiplier;
    public float attractSmoothSpeed;

    [Header("Repulsion")]
    public float maxRepelForceMultiplier;
    public float maxRepelDistance;
    public float repelSmoothSpeed;
}
