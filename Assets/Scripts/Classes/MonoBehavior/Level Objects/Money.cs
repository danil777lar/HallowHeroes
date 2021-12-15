using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Money : PlatformAttachment
{
    [SerializeField] private int _value;
    [SerializeField] private SpriteRenderer _coinSprite;


    public override void AnchorToPlatform(float surfacePosition)
    {
        Vector3 targetPosition = Vector3.up * surfacePosition;
        float leftScreenSide = Camera.main.ViewportToWorldPoint(Vector3.zero).x + 1f;
        float rightScreenSide = Camera.main.ViewportToWorldPoint(Vector3.right).x - 1f;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            ProcessPanel.Default.PlayCoinAnim(_value, _coinSprite.transform.position);
            _coinSprite.DOColor(new Color(1f, 1f, 1f, 0f), 0.25f)
                .OnComplete(() => Destroy(gameObject));
        }
    }
}
