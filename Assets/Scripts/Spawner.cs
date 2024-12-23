using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Camera _mainCamera;
    private Transform _transform;

    public Vector3 Position => _transform.position;
    
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
}