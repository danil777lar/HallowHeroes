using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private int _jumpsLeft;
    private int _horizontalDirection = 1;
    private Camera _camera;


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
        base.FixedUpdate();

        HorizontalMovement();
    }

    private void OnEnable()
    {
        ProcessPanel.Default.OnTap += HandleOnUserTap;
    }

    private void OnDestroy()
    {
        ProcessPanel.Default.OnTap -= HandleOnUserTap;
    }


    private void HandleOnUserTap()
    {
        if (_jumpsLeft > 0)
        {
            _jumpsLeft--;
            Jump(_jumpForce);
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
