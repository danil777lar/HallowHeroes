using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlatformAttachment : MonoBehaviour
{
    protected float boundsWidth;

    public virtual void AnchorToPlatform(float surfacePosition) 
    {
        Vector3 targetPosition = Vector3.up * surfacePosition;
        float leftScreenSide = Camera.main.ViewportToWorldPoint(Vector3.zero).x + boundsWidth / 2f;
        float rightScreenSide = Camera.main.ViewportToWorldPoint(Vector3.right).x - boundsWidth / 2f;
        float rand = Random.Range(0f, 1f);
        targetPosition.x = Mathf.Lerp(leftScreenSide, rightScreenSide, rand);
        if (rand < 0.5f)
        {
            Vector3 targetScale = transform.localScale;
            targetScale.x *= -1f;
            transform.localScale = targetScale;
        }
        transform.localPosition = targetPosition;
    }
}
