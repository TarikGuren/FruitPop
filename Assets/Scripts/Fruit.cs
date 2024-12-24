using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Fruit : MonoBehaviour
{
    private FruitDefinition _definition;
    private Transform _transform;
    private Rigidbody2D _rb;
    private SpriteRenderer _renderer;
    
    public FruitDefinition Definition => _definition;
    public Vector3 Velocity => _rb.velocity;
    
    public UnityAction<Fruit, Fruit> OnContact;
    

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
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

    public void Initialize(FruitDefinition def, bool extraMovement = false)
    {
        _definition = def;
        _transform.localScale = _definition.Scale;
        _renderer.sprite = _definition.Sprite;
        _rb.mass = _definition.Mass;
        if (extraMovement)
        {
            _rb.velocity = Vector3.one * 0.5f;
            _transform.localScale = Vector3.zero;
            _transform.DOScale(_definition.Scale, 0.1f);
        }
    }

    private void Explode(Vector3 centerPoint)
    {
        var results = Physics2D.CircleCastAll(centerPoint, _definition.ExplosionRadius, Vector2.one);
        Debug.Log(results.Length);
        foreach (var hit in results)
        {
            if (hit.rigidbody == null) continue;
            var direction = hit.transform.position - centerPoint;
            if(direction.magnitude <= 0) continue;
            var exploisonForceByDistance = _definition.ExplosionStrength * direction.magnitude;
            
            Debug.Log($"Explosion on {hit.transform.name} with force {exploisonForceByDistance:F2}");
            hit.rigidbody.AddForce(direction.normalized * exploisonForceByDistance, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _definition.ExplosionRadius);
    }
}