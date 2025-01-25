using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControls : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    [SerializeField]
    private List<GameObject> _selectableMenuElements;

    private int _selectionIndex;

    [SerializeField]
    private GameObject _pointerBall;

    private GameObject _selectedMenuObject;

    public Color selectedColor, unselectedColor;

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private Image _whiteFadeInScreen;

    [SerializeField]
    private AudioClip _sfxMenuNavigate, _sfxMenuConfirm;

    public static MenuControls Instance;

    [SerializeField]
    public float xsway, xtime, ysway, ytime;

    public float elementAnimSpeed;

    public enum MenuState
    {
        None,
        StartGame,
        Credits,
        Exit
    }

    public MenuState CurrentState;

    private MenuState _previousState;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        _selectionIndex = 0;
        if(_selectableMenuElements.Count == 0)
        {
            Debug.LogWarning("No menu elements");
        }
        _selectedMenuObject = _selectableMenuElements[0];
        _selectedMenuObject.GetComponent<MenuSelectable>().ToggleSelect();
        _audioSource.clip = _sfxMenuNavigate;
        print(_selectableMenuElements.Count);
    }

    

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) 
        {
            _pointerBall.GetComponent<PointerBall>().isIntroPhase = false;
            _audioSource.Play();
            _selectionIndex++;
            if (_selectionIndex > _selectableMenuElements.Count - 1)
            {
                _selectionIndex = 0;
            }
            HandleSelection();
        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {

            _pointerBall.GetComponent<PointerBall>().isIntroPhase = false;
            _audioSource.Play();
            _selectionIndex--;
            if (_selectionIndex < 0)
            {
                _selectionIndex = _selectableMenuElements.Count - 1;
            }
            HandleSelection();
        }

        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            if (CurrentState != MenuState.None) return;

            StartCoroutine(HandleMenuSelectDelay(_selectedMenuObject.GetComponent<MenuSelectable>().MenuState));
        }
    }

    private void HandleSelection()
    {
        _selectedMenuObject.GetComponent<MenuSelectable>().ToggleSelect(); //toggle old
        _selectedMenuObject = _selectableMenuElements[_selectionIndex];
        _selectedMenuObject.GetComponent<MenuSelectable>().ToggleSelect(); //toggle new
        _pointerBall.GetComponent<PointerBall>().MoveSelectionBall(_selectedMenuObject.GetComponent<MenuSelectable>().SquarePosition());
    }

    IEnumerator HandleMenuSelectDelay(MenuState NextState)
    {
        //menu select sound here
        _audioSource.clip = _sfxMenuConfirm;
        _audioSource.Play();
        yield return new WaitForSeconds(1f);
        CurrentState = NextState;
        CheckStates();
    }


    public void CheckStates()
    {
        if (_previousState == CurrentState) return;
        _previousState = CurrentState;
        switch (CurrentState)
        {
            case MenuState.StartGame:
            {
                SceneManager.LoadScene("SampleScene_Oskar");
                break;
            }
            case MenuState.Credits:
            {
                break;
            }
            case MenuState.Exit:
            {
                break;
            }
        }
        _audioSource.clip = _sfxMenuNavigate;
        //_pausedCanvasParent.SetActive(CurrentState == GameState.Paused);
        //_inPlayPanel.SetActive(CurrentState == GameState.InPlay);
    }

}
