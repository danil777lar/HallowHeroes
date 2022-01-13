using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PlayerMovement : PlatformerGravity
{
    #region Singleton
    static private PlayerMovement _default;
    static public PlayerMovement Defualt => _default;
    #endregion

    [Header("Jump")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private int _jumpCount;
    [Header("Movement")]
    [SerializeField] private float _horizontalSpeed;
    [SerializeField] private Transform _playerSprite;
    [Header("Effects")]
    [SerializeField] private ParticleSystem _punchParts;

    private bool _isDead;
    private int _jumpsLeft;
    private int _horizontalDirection = 1;
    private Vector2 _positionLastFrame;
    private Camera _camera;

    public Action OnJump;
    

    private void Awake()
    {
        _default = this;
        _jumpsLeft = _jumpCount;
    }

    protected override void Start()
    {
        base.Start();

        _camera = Camera.main;
        OnGrounded += () => _jumpsLeft = _jumpCount;
    }

    protected override void FixedUpdate()
    {
        if (!_isDead)
        {
            base.FixedUpdate();
            HorizontalMovement();
        }
        else
        {
            VelocityY -= 9.8f * _gravityScale * Time.fixedDeltaTime;
            Vector2 targetPosition = transform.position;
            targetPosition.y += VelocityY * Time.fixedDeltaTime;
            transform.position = targetPosition;

            Vector2 dir = _positionLastFrame - (Vector2)transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            angle += _horizontalDirection == -1 ? 180f : 0f;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Vector3.forward * angle), Time.fixedDeltaTime * 5f);
            
            _positionLastFrame = transform.position;
        }
    }

    private void OnEnable()
    {
        ProcessPanel.Default.OnTap += HandleOnUserTap;
    }

    private void OnDestroy()
    {
        ProcessPanel.Default.OnTap -= HandleOnUserTap;
    }


    public void PlayDeathAnim(Collider2D other)
    {
        _isDead = true;
        _positionLastFrame = other.transform.TransformPoint((other as BoxCollider2D).offset);
        Vector2 normal = transform.TransformPoint(_collider.offset) - other.transform.TransformPoint((other as BoxCollider2D).offset);
        
        ParticleSystem parts = Instantiate(_punchParts);
        parts.transform.SetParent(LevelManager.Default.transform);
        parts.transform.position = transform.TransformPoint(_collider.offset) + ((Vector3)normal / 2f);
        Destroy(parts.gameObject, parts.main.duration);
        
        normal = normal.normalized;
        VelocityY = normal.y * 10f;
        transform.DOMoveX(transform.position.x + Mathf.Sign(normal.x) * 3f, 1f)
            .SetEase(Ease.OutSine)
            .SetUpdate(UpdateType.Fixed);
    }


    private void HandleOnUserTap()
    {
        if (_jumpsLeft > 0)
        {
            _jumpsLeft--;
            VelocityY = _jumpForce;
            OnJump?.Invoke();
            SoundHolder.Default.PlayFromSoundPack("Jump");
        }
    }

    private void HorizontalMovement()
    {
        float deltaPosition = _horizontalSpeed * Time.fixedDeltaTime * _horizontalDirection;
        float colliderBoundX = _horizontalDirection > 0 ? _collider.bounds.max.x : _collider.bounds.min.x;
        float cameraBoundX = _camera.ViewportToWorldPoint(new Vector3(_horizontalDirection > 0 ? 0f : 1f, 0f, 0f)).x;
        if (Mathf.Abs(colliderBoundX) > Mathf.Abs(cameraBoundX))
        {
            _horizontalDirection *= -1;
            Vector3 targetScale = _playerSprite.localScale;
            targetScale.x *= -1f;
            _playerSprite.localScale = targetScale;
        }
        Vector3 targetPosition = transform.position;
        targetPosition.x += deltaPosition;
        transform.position = targetPosition;
    }
}
