using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollisionDemo : MonoBehaviour
{
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private float _moveSpeed = 3f;

    [SerializeField] private Input _input;
    
    private int _maxBounces = 5;
    private float _skinWidth = 0.015f;
    private float _maxSlope = 55f;
    
    private Bounds _bounds;

    private bool _isGrounded = false;

    private Vector3 _moveInput;
    
    private void Start()
    {
        _input.MoveInput += OnMoveInput;
        Debug.Log("start");
    }

    private void Update()
    {
        _bounds = _collider.bounds;
        _bounds.Expand(-2 * _skinWidth);
        
        Move(_moveInput.normalized * (_moveSpeed * Time.deltaTime));
    }

    public void Move(Vector3 moveAmount)
    {
        Vector3 gravity = Vector3.down * 9.8f * Time.deltaTime;
        
        moveAmount = CollideAndSlide(moveAmount, transform.position, 0, false, moveAmount);
        moveAmount += CollideAndSlide(gravity, transform.position + moveAmount, 0, true, gravity);

        transform.Translate(moveAmount);
    }

    private Vector3 CollideAndSlide(Vector3 velocity, Vector3 position, int depth, bool gravityPass, Vector3 velocityInit)
    {
        if (depth >= _maxBounces)
            return Vector3.zero;

        float distance = velocity.magnitude + _skinWidth;
        
        RaycastHit hit;
        
        GetCapsuleStartEndPoints(_collider, out Vector3 p1, out Vector3 p2);

        if (Physics.CapsuleCast(p1, p2, _collider.radius, velocity.normalized, out hit, distance))
        {
            Debug.DrawLine(_collider.center, hit.point, Color.magenta);
            
            Vector3 snapToSurface = velocity.normalized * (hit.distance - _skinWidth);
            Vector3 leftover = velocity - snapToSurface;
            float angle = Vector3.Angle(Vector3.up, hit.normal);
            
            if (snapToSurface.magnitude <= _skinWidth)
                snapToSurface = Vector3.zero;

            if (angle <= _maxSlope)
            {
                if (gravityPass)
                    return snapToSurface;
                
                leftover = ProjectAndScale(leftover, hit.normal);
            }
            else
            {
                float scale = 1 - Vector3.Dot(
                    new Vector3(hit.normal.x, 0f, hit.normal.z).normalized,
                    -new Vector3(velocityInit.x, 0f, velocityInit.z).normalized);

                if (_isGrounded && !gravityPass)
                {
                    leftover = ProjectAndScale(
                        new Vector3(leftover.x, 0f, leftover.z),
                        new Vector3(hit.normal.x, 0f, hit.normal.z)).normalized;
                    leftover *= scale;
                }
                else
                {
                    leftover = ProjectAndScale(leftover, hit.normal) * scale;
                }
            }

            return snapToSurface + CollideAndSlide(leftover, position + snapToSurface, depth + 1, gravityPass, velocityInit);
        }
        
        return velocity;
    }

    private Vector3 ProjectAndScale(Vector3 vector, Vector3 normal)
    {
        float magnitude = vector.magnitude;
        vector = Vector3.ProjectOnPlane(vector, normal).normalized;
        return vector *= magnitude;
    }

    private void GetCapsuleStartEndPoints(CapsuleCollider capsule, out Vector3 start, out Vector3 end)
    {
        float halfLineLength = Mathf.Max(0f, (capsule.height * 0.5f) - capsule.radius);
        
        Vector3 localDirection = Vector3.up;
        if (capsule.direction == 0)
            localDirection = Vector3.right;
        else if (capsule.direction == 2)
            localDirection = Vector3.forward;

        Vector3 localStart = capsule.center + (localDirection * halfLineLength);
        Vector3 localEnd = capsule.center - (localDirection * halfLineLength);

        start = capsule.transform.TransformPoint(localStart);
        end = capsule.transform.TransformPoint(localEnd);
    }

    private void OnMoveInput(Vector2 moveInput)
    {
        _moveInput = new Vector3(moveInput.x, 0f, moveInput.y);
    }
}
