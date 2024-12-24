using System;
using UnityEngine;
using UnityEngine.Events;

public class Fruit : MonoBehaviour
{
    [SerializeField] private FruitDefinition _definition;
    [SerializeField] private float _explosionRadius = 1f;
    [SerializeField] private float _explosionStrength = 1f;

    public FruitDefinition Definition => _definition;
    public Rigidbody2D Rigidbody => _rb;
    public Vector3 Velocity => _rb.velocity;
    
    public UnityAction<Fruit, Fruit> OnContact;
    
    private Rigidbody2D _rb;
    private Transform _transform;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _transform = transform;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var otherFruit = other.collider.GetComponent<Fruit>();
        if (otherFruit == null) return;

        if (otherFruit.Definition == _definition)
        {
            otherFruit.OnContact = null;
            OnContact?.Invoke(this, otherFruit);
            Explode(other.GetContact(0).point);
            Destroy(gameObject);
        }
    }

    public void Initialize(Vector3 oldVelocity)
    {
        _rb.velocity = Vector3.right * 0.5f;
    }

    private void Explode(Vector3 centerPoint)
    {
        var results = Physics2D.CircleCastAll(_transform.position, _explosionRadius, Vector2.one);
        Debug.Log(results.Length);
        foreach (var hit in results)
        {
            if (hit.rigidbody == null) continue;
            var direction = hit.transform.position - _transform.position;
            if(direction.magnitude <= 0) continue;
            var exploisonForceByDistance = _explosionStrength * direction.magnitude;
            
            Debug.Log($"Explosion on {hit.transform.name} with force {exploisonForceByDistance:F2}");
            hit.rigidbody.AddForce(direction.normalized * exploisonForceByDistance, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}