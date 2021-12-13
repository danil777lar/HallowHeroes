using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Singleton
    private static UIManager _default;
    public static UIManager Default => _default;
    #endregion

    public enum State {Start, Shop, Process, Fail}

    [SerializeField] private Panel _startPanel;
    [SerializeField] private Panel _shopPanel;
    [SerializeField] private Panel _processPanel;
    [SerializeField] private Panel _failPanel;

    private Dictionary<State, Panel> _stateToPanel;
    private State _curentState;

    public Action<State, State> OnStateChanged;
    public State CurentState 
    {
        get => _curentState;
        set
        {
            if (_curentState != value)
            {
                _stateToPanel[value].ShowPanel();
                _stateToPanel[_curentState].HidePanel();
                OnStateChanged?.Invoke(_curentState, value);
                _curentState = value;
            }
        }
    }

    private void Awake()
    {
        _default = this;

        _stateToPanel = new Dictionary<State, Panel>();
        _stateToPanel.Add(State.Start, _startPanel);
        _stateToPanel.Add(State.Shop, _shopPanel);
        _stateToPanel.Add(State.Process, _processPanel);
        _stateToPanel.Add(State.Fail, _failPanel);
    }
}
