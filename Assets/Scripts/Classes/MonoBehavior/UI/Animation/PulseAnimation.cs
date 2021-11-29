using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PulseAnimation : MonoBehaviour
{
    [SerializeField] private float _targetScale;
    [SerializeField] private float _animDuration;


    private void Start()
    {
        transform.DOScale(_targetScale, _animDuration / 2f)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
