using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKEnvironmentObject : PlatformAttachment
{
    [SerializeField] private SpriteRenderer _rend;
    [SerializeField] private Gradient _color;
    [SerializeField] private List<Transform> _ikTargets;

    private bool _isAttached;
    private int _seed; 
    private Dictionary<Transform, Vector3> _defaultPositions = new Dictionary<Transform, Vector3>();


    private void Start()
    {
        _rend.color = _color.Evaluate(Random.Range(0f, 1f));
        _seed = Random.Range(0, 1000);
    }

    private void Update()
    {
        if (_isAttached)
        {
            foreach (var target in _ikTargets)
            {
                Vector3 position = _defaultPositions[target];
                position.x += (Mathf.PerlinNoise(Time.time / 2f, _seed) - 0.5f) * 0.4f;
                position.y += (Mathf.PerlinNoise(_seed, Time.time / 2f) - 0.5f) * 0.4f;
                target.position = position;
            }
        }
    }


    public override void AnchorToPlatform(float surfacePosition)
    {
        base.AnchorToPlatform(surfacePosition);
        foreach (var target in _ikTargets)
        {
            _defaultPositions.Add(target, target.position);
        }
        _isAttached = true;
    }
}