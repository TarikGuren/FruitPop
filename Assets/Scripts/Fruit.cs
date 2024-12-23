using System;
using UnityEngine;
using UnityEngine.Events;

public class Fruit : MonoBehaviour
{
    [SerializeField] private FruitDefinition _definition;

    public FruitDefinition Definition => _definition;
    public Rigidbody2D Rigidbody => _rb;
    public Vector3 Velocity => _rb.velocity;
    
    public UnityAction<Fruit, Fruit> OnContact;
    
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var otherFruit = other.collider.GetComponent<Fruit>();
        if (otherFruit == null) return;

        if (otherFruit.Definition == _definition)
        {
            otherFruit.OnContact = null;
            OnContact?.Invoke(this, otherFruit);
            Destroy(gameObject);
        }
    }

    public void CopyPhysics(Vector3 oldVelocity)
    {
        _rb.velocity = oldVelocity;
    }
}