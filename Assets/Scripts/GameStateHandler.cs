using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStateHandler : MonoBehaviour
{
    // Start is called before the first frame update

    public enum GameState
    {
        None,
        StartScene,
        InPlay,
        Paused,
    }

    [SerializeField]
    private GameObject _pausedCanvasParent, _inPlayPanel;

    [SerializeField]
    private TMP_Text _playerInfo;

    private GameManager _gm;

    public GameState CurrentState;
    private GameState _previousState { get; set; }


    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        _previousState = GameState.None;
        _gm = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckStates();
    }

    public void CheckStates()
    {
        if (_previousState == CurrentState) return;
        _previousState = CurrentState;
        switch (CurrentState)
        {
            case GameState.StartScene:
            {
                _gm.IsPlayerFrozen = true;
                break;
            }
            case GameState.Paused:
            {
                break;
            }
            case GameState.InPlay:
            {
                _gm.IsPlayerFrozen = false;
                break;
            }
        }
        //_pausedCanvasParent.SetActive(CurrentState == GameState.Paused);
        //_inPlayPanel.SetActive(CurrentState == GameState.InPlay);
    }
}