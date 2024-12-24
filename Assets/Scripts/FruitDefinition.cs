using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Game")]
public class FruitDefinition : ScriptableObject
{
    public string FruitName;
    
    public Sprite Sprite;
    public Vector2 Scale = Vector2.one;
    [FormerlySerializedAs("Weight")] public float Mass = 1f;
    
    public float ExplosionRadius = 1f;
    public float ExplosionStrength = 1f;
    
    public FruitDefinition FruitEvolution;
}