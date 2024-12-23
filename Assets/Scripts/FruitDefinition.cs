using UnityEngine;

[CreateAssetMenu(menuName = "Game")]
public class FruitDefinition : ScriptableObject
{
    public string FruitName;
    public Sprite Sprite;
    public Fruit Prefab;
    public FruitDefinition FruitEvolution;
}