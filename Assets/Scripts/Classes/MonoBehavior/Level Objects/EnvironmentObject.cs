using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentObject : PlatformAttachment
{
    [SerializeField] private SpriteRenderer _rend;
    [SerializeField] private List<Sprite> _sprites;
    [SerializeField] private Gradient _color;

    private void Start()
    {
        _rend.sprite = _sprites[Random.Range(0, _sprites.Count)];
        _rend.color = _color.Evaluate(Random.Range(0f, 1f));
    }
}
