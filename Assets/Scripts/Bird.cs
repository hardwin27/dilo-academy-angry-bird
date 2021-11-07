using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bird : MonoBehaviour
{
    public enum BirdState { Idle, Thrown, HitSomething }
    public GameObject Parent;
    public Rigidbody2D Rigidbody;
    public CircleCollider2D Collider;

    public UnityAction OnBirdDestroyed = delegate { };
    public UnityAction<Bird> OnBirdShot = delegate { };

    private BirdState _state;
    public BirdState State { get { return _state; } }
    private float _minVelocity = 0.05f;
    private bool _flagDestroy = false;

    private void Start()
    {
        Rigidbody.bodyType = RigidbodyType2D.Kinematic;
        Collider.enabled = false;
        _state = BirdState.Idle;
    }

    private void FixedUpdate()
    {
        if(_state == BirdState.Idle && Rigidbody.velocity.sqrMagnitude >= _minVelocity)
        {
            _state = BirdState.Thrown;
        }

        if((_state == BirdState.Thrown || _state == BirdState.HitSomething) && Rigidbody.velocity.sqrMagnitude < _minVelocity && !_flagDestroy)
        {
            //Start to destroy the bird when it stop moving after 2 second
            _flagDestroy = true;
            StartCoroutine(DestroyAfter(2));
        }
    }

    private void OnDestroy()
    {
        if(_state == BirdState.Thrown || _state == BirdState.HitSomething)
        {
            OnBirdDestroyed();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _state = BirdState.HitSomething;
    }

    //Destroy gameObject after delay
    private IEnumerator DestroyAfter(float second)
    {
        yield return new WaitForSeconds(second);
        Destroy(gameObject);
    }

    public void MoveTo(Vector2 target, GameObject parent)
    {
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.position = target;
    }

    public void Shoot(Vector2 velocity, float distance, float speed)
    {
        Collider.enabled = true;
        Rigidbody.bodyType = RigidbodyType2D.Dynamic;
        Rigidbody.velocity = velocity * speed * distance;
        OnBirdShot(this);
    }

    public virtual void OnTap()
    {

    }
}
