using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class IKTargetFixer : MonoBehaviour
{
    [SerializeField] private float _animationSpeed;
    [SerializeField] private float _animationScale;

    private bool _inited;
    private int _seed;
    private Vector3 _defaultPosition;

    private void Awake()
    {
        GetComponentInParent<PlatformAttachment>().OnAttached += Init;
    }
    
    private void Update()
    {
        if (_inited)
        {
            Vector3 targetPosition = _defaultPosition;
            targetPosition.x += (Mathf.PerlinNoise(Time.time * _animationSpeed, _seed) - 0.5f) * 2f * _animationScale;
            targetPosition.y += (Mathf.PerlinNoise(_seed, Time.time * _animationSpeed) - 0.5f) * 2f * _animationScale;
            transform.position = targetPosition;
        }
    }


    private void Init()
    {
        Debug.Log("Init");
        _inited = true;
        _seed = Random.Range(0, 10000);
        _defaultPosition = transform.position;
    }
}
