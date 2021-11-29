using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifecycle : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerAnimation _playerAnimation;

    private bool _failed;


    private void OnEnable()
    {
        UIManager.Default.OnStateChanged += HandleOnUIStateChanged;
    }

    private void OnDisable()
    {
        UIManager.Default.OnStateChanged -= HandleOnUIStateChanged;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemie")) 
        {
            if (!_failed)
            {
                _failed = true;
                UIManager.Default.CurentState = UIManager.State.Fail;
                _playerMovement.enabled = false;
                _playerAnimation.PlayFailAnim(transform.position - collision.transform.position);
            }
        }
    }


    private void HandleOnUIStateChanged(UIManager.State oldState, UIManager.State newState) 
    {
        if (newState == UIManager.State.Process) 
        {
            _playerMovement.enabled = true;
            _playerAnimation.enabled = true;
        }
    }
}
