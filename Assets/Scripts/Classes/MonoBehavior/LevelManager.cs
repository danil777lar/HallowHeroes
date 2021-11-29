using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    #region Singleton
    static private LevelManager _default;
    static public LevelManager Default => _default;
    #endregion

    [SerializeField] private LevelGenerator _levelPrefab;

    public Action OnRestart;


    private void Awake()
    {
        _default = this;
    }

    public void Restart() 
    {
        OnRestart?.Invoke();
        Destroy(transform.GetChild(0).gameObject);
        Instantiate(_levelPrefab).transform.SetParent(transform);
    }
}
