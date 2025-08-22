using UnityEngine;

[CreateAssetMenu(fileName = "NewPlanetGravity", menuName = "Scriptable Objects/GravitySettings")]
public class GravitySettings : ScriptableObject
{
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpForceMultiplier = 1f;

    public float Gravity => gravity;
    public float JumpForceMultiplier => jumpForceMultiplier;
}
