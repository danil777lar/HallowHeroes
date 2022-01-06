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
        List<Transform> childList = new List<Transform>();
        foreach (var v in transform)
            childList.Add(v as Transform);
        foreach (var v in childList)
            DestroyImmediate(v.gameObject);
        
        Instantiate(_levelPrefab).transform.SetParent(transform);
    }
}
