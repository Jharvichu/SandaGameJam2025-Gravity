using UnityEngine;

[CreateAssetMenu(fileName = "Hole", menuName = "Scriptable Objects/Hole")]
public class Hole : ScriptableObject
{
    [SerializeField] private string name;
    [SerializeField] private GameObject holePrefab;
    [SerializeField] private Sprite holeSprite;

    public string Name => name;
    public GameObject HolePrefab => holePrefab;
    public Sprite HoleSprite => holeSprite;

}
