using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifecycle : MonoBehaviour
{
    [SerializeField] private Transform _bodyHolder;
    [SerializeField] private CharacterHolder _characterHolder;
    [SerializeField] private PlayerMovement _playerMovement;

    private bool _failed;


    private void Awake()
    {
        GameObject instance = Instantiate(_characterHolder.Current.characterPrefab);
        instance.transform.SetParent(_bodyHolder);
        instance.transform.localPosition = Vector3.zero;
    }

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
            }
        }
    }


    private void HandleOnUIStateChanged(UIManager.State oldState, UIManager.State newState) 
    {
        if (newState == UIManager.State.Process) 
        {
            _playerMovement.enabled = true;
        }
    }
}
