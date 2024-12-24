using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<Fruit> _fruitPrefabs;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private SpriteRenderer _upcomingFruitVisual;
    
    private Fruit _nextFruit;
    private Fruit _upcomingFruit;
    
    private void Start()
    {
        _upcomingFruit = _fruitPrefabs[Random.Range(0, _fruitPrefabs.Count)];
        SetNextFruit();
    }

    private void SetNextFruit()
    {
        _nextFruit = _upcomingFruit;
        _spawner.SetVisual(_nextFruit);
        
        _upcomingFruit = _fruitPrefabs[Random.Range(0, _fruitPrefabs.Count)];
        
        _upcomingFruitVisual.sprite = _upcomingFruit.Definition.Sprite;
        _upcomingFruitVisual.transform.localScale = _upcomingFruit.transform.localScale;
    }

    private void OnContact(Fruit first, Fruit second)
    {
        if (first.Definition.FruitEvolution == null) return;
        StartCoroutine(DelayedSpawn(first.Definition.FruitEvolution.Prefab, first.Velocity, first.transform.position, second.transform.position));
        first.OnContact -= OnContact;
    }

    private IEnumerator DelayedSpawn(Fruit prefab, Vector3 velocity, Vector3 first, Vector3 second)
    {
        yield return new WaitForSeconds(0.1f);
        var center = second + (first - second) * 0.5f;
        SpawnFruit(prefab, center, first);
    }

    private void SpawnFruit(Fruit fruit, Vector3 position, Vector3? old = null)
    {
        var newFruit = Instantiate(fruit, position, Quaternion.identity);
        newFruit.OnContact += OnContact;

        if (old.HasValue)
        {
            newFruit.Initialize(old.Value);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            SpawnFruit(_nextFruit, _spawner.Position);
            SetNextFruit();
        }
    }
}