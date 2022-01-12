using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(NamedAnimancerComponent))]
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private AnimationClip _idleAnim;
    [SerializeField] private AnimationClip _runAnim;
    [SerializeField] private AnimationClip _jumpAnim;
    [SerializeField] private AnimationClip _deathAnim;

    private bool _computePause = false;
    private NamedAnimancerComponent _animancer;
    private PlayerMovement _movement;

    private void Start()
    {
        _animancer = GetComponent<NamedAnimancerComponent>();
        _movement = GetComponentInParent<PlayerMovement>();
        if (_movement)
        {
            _movement.OnJump += () =>
            {
                _animancer.Stop(_jumpAnim);
                _animancer.Play(_jumpAnim, 0.1f);
                StopAllCoroutines();
                StartCoroutine(ComputePauseCoroutine(0.33f));
            };
        }

        PlayerLifecycle lifecycle = GetComponentInParent<PlayerLifecycle>();
        if (lifecycle)
        {
            lifecycle.OnFail += () =>
            {
                _animancer.Play(_deathAnim, 0.1f);
                _computePause = true;
            };
        }
    }


    private void FixedUpdate()
    {
        if (!_computePause)
        {
            ComputeAnimation(out AnimationClip clip, out float fade);
            _animancer.Play(clip, fade);
        }
    }


    private void ComputeAnimation(out AnimationClip clip, out float fade)
    {
        fade = 0.5f;
        clip = _idleAnim;

        if (_movement && _movement.enabled && _movement.IsGrounded)
        {
            fade = 0.1f;
            clip = _runAnim;
        }
    }

    private IEnumerator ComputePauseCoroutine(float delay)
    {
        _computePause = true;
        yield return new WaitForSeconds(delay);
        _computePause = false;
    }
}
