using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _verticalOffset;
    [SerializeField] private float _movementHardness;
    [SerializeField] private float _paralaxStripWidth;
    [SerializeField] private float _paralaxStripOffsetStrenght;
    [SerializeField] private List<SpriteRenderer> _paralaxStrips;

    private BoxCollider2D _back;
    private Material _paralaxStripMaterial;


    private void Awake()
    {
        Canvas canvas = UIManager.Default.GetComponentInChildren<Canvas>();
        canvas.worldCamera = Camera.main;

        Vector2 screenSizeWorld = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 0f)) - Camera.main.ViewportToWorldPoint(Vector3.zero);

        _paralaxStripMaterial = Instantiate(_paralaxStrips[0].sharedMaterial);
        for (int i = 0; i < _paralaxStrips.Count; i++) 
        {
            _paralaxStrips[i].sharedMaterial = _paralaxStripMaterial;
            _paralaxStrips[i].size = new Vector2(_paralaxStripWidth, screenSizeWorld.y);
            float xPosition = screenSizeWorld.x / 2f * (i == 0 ? -1f : 1f);
            _paralaxStrips[i].transform.localPosition = new Vector3(xPosition, 0f, 10f);
        }
    }

    private void FixedUpdate()
    {
        if (UIManager.Default.CurentState == UIManager.State.Process)
        {
            Vector3 targetPosition = transform.position;
            targetPosition.y = PlayerMovement.Defualt.transform.position.y + _verticalOffset;
            float deltaY = transform.position.y - targetPosition.y;
            Vector2 newTextureOffset = _paralaxStripMaterial.GetTextureOffset("_MainTex") + Vector2.up * deltaY * _paralaxStripOffsetStrenght;
            _paralaxStripMaterial.SetTextureOffset("_MainTex", newTextureOffset);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * _movementHardness); 
        }
    }


    public void SetColors(BoxCollider2D backgroundPrefab, Color stripColor, float duration) 
    {
        foreach (var strip in _paralaxStrips)
        {
            strip.DOColor(stripColor, duration);
        }

        if (_back) Destroy(_back.gameObject);

        _back = Instantiate(backgroundPrefab);
        _back.transform.SetParent(transform);
        _back.transform.localPosition = Vector3.forward * 10f;
        Vector3 scale = Vector3.one;
        scale.x = (Camera.main.ViewportToWorldPoint(Vector3.one).x - Camera.main.ViewportToWorldPoint(Vector3.zero).x) / _back.size.x;
        scale.y = (Camera.main.ViewportToWorldPoint(Vector3.one).y - Camera.main.ViewportToWorldPoint(Vector3.zero).y) / _back.size.y;
        _back.transform.localScale = scale;
    }
}
