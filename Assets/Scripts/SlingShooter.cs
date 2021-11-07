using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShooter : MonoBehaviour
{
    public CircleCollider2D Collider;
    public LineRenderer Trajectory;
    private Vector2 _startPos;

    [SerializeField] private float _radius = 0.75f;
    [SerializeField] private float _throwSpeed = 30f;

    private Bird _bird;

    private void Start()
    {
        _startPos = transform.position;
    }

    private void OnMouseUp()
    {
        //Disable the collider when we launch the bird
        Collider.enabled = false;
        Vector2 velocity = _startPos - (Vector2)transform.position;
        float distance = Vector2.Distance(_startPos, transform.position);

        _bird.Shoot(velocity, distance, _throwSpeed);

        //Reset slingshot position back;
        gameObject.transform.position = _startPos;
        Trajectory.enabled = false;
    }

    private void OnMouseDrag()
    {
        Vector2 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Calculate to make sure slingshot dragged only to a certain radius
        Vector2 dir = p - _startPos;
        if(dir.sqrMagnitude > _radius)
        {
            dir = dir.normalized * _radius;
        }
        transform.position = _startPos + dir;

        float distance = Vector2.Distance(_startPos, transform.position);

        if(!Trajectory.enabled)
        {
            Trajectory.enabled = true;
        }

        DisplayTrajectory(distance);
    }

    public void InitiateBird(Bird bird)
    {
        _bird = bird;
        _bird.MoveTo(gameObject.transform.position, gameObject);
        Collider.enabled = true;
    }

    private void DisplayTrajectory(float distance)
    {
        if(_bird == null)
        {
            return;
        }

        Vector2 velocity = _startPos - (Vector2)transform.position;
        int segmentCount = 5;
        Vector2[] segments = new Vector2[segmentCount];

        //mouse position become the start position of the trajectory
        segments[0] = transform.position;

        //Early velocity
        Vector2 segVelocity = velocity * _throwSpeed * distance;

        for(int i = 1; i < segmentCount; i++)
        {
            float elapsedTime = i * Time.fixedDeltaTime * 5;
            segments[i] = segments[0] + segVelocity * elapsedTime + 0.5f * Physics2D.gravity * Mathf.Pow(elapsedTime, 2);
        }

        Trajectory.positionCount = segmentCount;
        for(int i = 0; i < segmentCount; i++)
        {
            Trajectory.SetPosition(i, segments[i]);
        }
    }
}
