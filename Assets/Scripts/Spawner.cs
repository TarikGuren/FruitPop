using UnityEngine;
using UnityEngine.Serialization;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform _spawnTransform;
    [SerializeField] private SpriteRenderer _spawnRenderer;
    
    private Transform _transform;
    private Camera _mainCamera;

    public Vector3 Position => _spawnTransform.position;
    
    private void Awake()
    {
        _transform = transform;
        _mainCamera = Camera.main;
    }
    
    private void Update()
    {
        var mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

        var spawnerPos = _transform.position;
        spawnerPos.x = mousePosition.x;
        _transform.position = spawnerPos;
    }

    public void SetVisual(Fruit fruit)
    {
        _spawnRenderer.sprite = fruit.Definition.Sprite;
        _spawnTransform.localScale = fruit.transform.localScale;
    }
}