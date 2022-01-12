using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlatformerGravity : MonoBehaviour
{
    [Header("Gravity")]
    [SerializeField] protected float _gravityScale;

    private ContactFilter2D _filter;
    private Transform _defaultParent;
    protected Rigidbody2D _rb;
    protected BoxCollider2D _collider;

    public bool IsGrounded { get; private set; }
    public float VelocityY { get; protected set; }
    public Action OnGrounded;


    protected virtual void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.isKinematic = true;
        _defaultParent = transform.parent;

        _filter = new ContactFilter2D();
        _filter.layerMask = LayerMask.GetMask("Landable");
        _filter.useLayerMask = true;
    }

    protected virtual void FixedUpdate()
    {
        if (IsGrounded)
        {
            VelocityY = Mathf.Max(VelocityY, 0f);
        }
        else
        {
            VelocityY -= 9.8f * _gravityScale * Time.fixedDeltaTime;
        }

        bool isGroundedThisFrame = false;
        float deltaPosition = VelocityY * Time.fixedDeltaTime;
        float moveDistance = Mathf.Abs(deltaPosition);
        RaycastHit2D[] contacts = new RaycastHit2D[5];
        for (int i = 0; i < _rb.Cast(Vector2.down, _filter, contacts, moveDistance); i++)
        {
            bool isNormalUp = contacts[i].normal == Vector2.up;
            bool isPointOutCollider = contacts[i].point.y <= _collider.bounds.min.y;
            if (isNormalUp && isPointOutCollider && VelocityY <= 0f)
            {
                isGroundedThisFrame = true;
                if (!IsGrounded)
                {
                    if (contacts[i].rigidbody != null && contacts[i].rigidbody.TryGetComponent(out ILandableSurface surface))
                    {
                        surface.LandOn();
                        transform.SetParent(contacts[i].rigidbody.transform);
                    }
                    OnGrounded?.Invoke();
                }
                deltaPosition = Mathf.Min(contacts[i].distance, Mathf.Abs(deltaPosition)) * Mathf.Sign(deltaPosition);
            }
        }
        if (!isGroundedThisFrame && IsGrounded) 
        {
            transform.SetParent(_defaultParent);
        }
        IsGrounded = isGroundedThisFrame;
        transform.position = new Vector2(transform.position.x, transform.position.y + deltaPosition); 
    }
}
