using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemy : PlatformAttachment
{
    [SerializeField] private BoxCollider2D _collider;
    

    private void FixedUpdate()
    {
    }

    public override void AnchorToPlatform(float surfacePosition)
    {
        _boundsWidth = _collider.bounds.max.x - _collider.bounds.min.x;
        base.AnchorToPlatform(surfacePosition);
    }
}
