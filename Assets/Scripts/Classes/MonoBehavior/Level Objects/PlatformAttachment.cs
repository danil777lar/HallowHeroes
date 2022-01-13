using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class PlatformAttachment : MonoBehaviour
{
    protected float _boundsWidth;

    public Action OnAttached;
    

    public virtual void AnchorToPlatform(float surfacePosition) 
    {
        Vector3 targetPosition = Vector3.up * surfacePosition;
        float leftScreenSide = Camera.main.ViewportToWorldPoint(Vector3.zero).x + _boundsWidth / 2f;
        float rightScreenSide = Camera.main.ViewportToWorldPoint(Vector3.right).x - _boundsWidth / 2f;
        float rand = Random.Range(0f, 1f);
        targetPosition.x = Mathf.Lerp(leftScreenSide, rightScreenSide, rand);
        if (rand < 0.5f)
        {
            Vector3 targetScale = transform.localScale;
            targetScale.x *= -1f;
            transform.localScale = targetScale;
        }
        transform.localPosition = targetPosition;
        
        OnAttached?.Invoke();
    }
}
