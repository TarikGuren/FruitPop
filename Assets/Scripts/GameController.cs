using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<FruitDefinition> _spawnableFruits;
    [SerializeField] private  Fruit _fruitPrefab;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private SpriteRenderer _upcomingFruitVisual;
    
    private FruitDefinition _nextFruit;
    private FruitDefinition _upcomingFruit;
    
    private void Start()
    {
        _upcomingFruit = _spawnableFruits[Random.Range(0, _spawnableFruits.Count)];
        SetNextFruit();
    }

    private void SetNextFruit()
    {
        _nextFruit = _upcomingFruit;
        _spawner.SetVisual(_nextFruit);
        
        _upcomingFruit = _spawnableFruits[Random.Range(0, _spawnableFruits.Count)];
        
        _upcomingFruitVisual.sprite = _upcomingFruit.Sprite;
        _upcomingFruitVisual.transform.localScale = _upcomingFruit.Scale;
    }

    private void OnContact(Fruit first, Fruit second)
    {
        if (first.Definition.FruitEvolution == null) return;
        StartCoroutine(DelayedSpawn(first.Definition.FruitEvolution, first.transform.position, second.transform.position));
        first.OnContact -= OnContact;
    }

    private IEnumerator DelayedSpawn(FruitDefinition def, Vector3 first, Vector3 second)
    {
        yield return new WaitForSeconds(0.1f);
        var center = second + (first - second) * 0.5f;
        SpawnFruit(def, center, true);
    }

    private void SpawnFruit(FruitDefinition fruit, Vector3 position, bool extraMovement = false)
    {
        var newFruit = Instantiate(_fruitPrefab, position, Quaternion.identity);
        newFruit.Initialize(fruit, extraMovement);
        newFruit.OnContact += OnContact;
    }

    private bool _canSpawn = true;
    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && _canSpawn)
        {
            _canSpawn = false;
            StartCoroutine(ReleaseFruit());
        }
    }

    private IEnumerator ReleaseFruit()
    {
        SpawnFruit(_nextFruit, _spawner.Position);
        SetNextFruit();
        yield return new WaitForSeconds(0.5f);
        _canSpawn = true;
    }
}