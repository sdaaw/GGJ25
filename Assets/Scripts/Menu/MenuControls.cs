using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private AudioClip _sfxMenuNavigate, _sfxMenuGameStart, _sfxMenuSelect, _sfxMenuBack;

    public static MenuControls Instance;

    [SerializeField]
    public float xsway, xtime, ysway, ytime;

    public float elementAnimSpeed;

    public bool IsPreparingGame;

    [SerializeField]
    private GameObject _creditsPanel;
    public enum MenuState
    {
        None,
        Main,
        StartGame,
        Credits,
        Exit
    }

    private bool _switchingToCredits;

    public MenuState CurrentState;

    private MenuState _previousState;

    public float _whiteScreenAlpha;

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
            if (CurrentState == MenuState.Credits) return;
            _audioSource.clip = _sfxMenuNavigate;
            _pointerBall.GetComponent<PointerBall>().isIntroPhase = false;
            _audioSource.Play();
            _selectionIndex++;
            if (_selectionIndex > _selectableMenuElements.Count - 1)
            {
                _selectionIndex = 0;
            }
            HandleSelection();
        }

        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            if (CurrentState == MenuState.Credits) return;
            _audioSource.clip = _sfxMenuNavigate;
            _pointerBall.GetComponent<PointerBall>().isIntroPhase = false;
            _audioSource.Play();
            _selectionIndex--;
            if (_selectionIndex < 0)
            {
                _selectionIndex = _selectableMenuElements.Count - 1;
            }
            HandleSelection();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(CurrentState == MenuState.Credits)
            {
                StartCoroutine(HandleMenuSelectDelay(MenuState.Main, 0f, _sfxMenuBack));
            }
        }

        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            if (CurrentState != MenuState.Main) return;

            StartCoroutine(HandleMenuSelectDelay(_selectedMenuObject.GetComponent<MenuSelectable>().MenuState, 0f, _sfxMenuSelect));
        }

        if (CurrentState == MenuState.Credits)
        {
            if (_whiteFadeInScreen.color.a >= 1f && !_creditsPanel.activeSelf)
            {
                _whiteScreenAlpha = 1f;
                _creditsPanel.SetActive(true);
            }
            if (!_creditsPanel.activeSelf)
            {
                _whiteScreenAlpha += 0.7f * Time.deltaTime;
                _whiteFadeInScreen.color = new Color(_whiteFadeInScreen.color.r, _whiteFadeInScreen.color.g, _whiteFadeInScreen.color.b, _whiteScreenAlpha);
            } else
            {
                _whiteScreenAlpha -= 0.7f * Time.deltaTime;
                _whiteFadeInScreen.color = new Color(_whiteFadeInScreen.color.r, _whiteFadeInScreen.color.g, _whiteFadeInScreen.color.b, _whiteScreenAlpha);
            }
        }

        if(IsPreparingGame) 
        {
            _whiteScreenAlpha += 0.15f * Time.deltaTime;
            _whiteFadeInScreen.color = new Color(_whiteFadeInScreen.color.r, _whiteFadeInScreen.color.g, _whiteFadeInScreen.color.b, _whiteScreenAlpha);
            if(_whiteScreenAlpha >= 1f)
            {
                SceneManager.LoadScene("Intro");
            }
        }
    }

    private void HandleSelection()
    {
        _selectedMenuObject.GetComponent<MenuSelectable>().ToggleSelect(); //toggle old
        _selectedMenuObject = _selectableMenuElements[_selectionIndex];
        _selectedMenuObject.GetComponent<MenuSelectable>().ToggleSelect(); //toggle new
        _pointerBall.GetComponent<PointerBall>().MoveSelectionBall(_selectedMenuObject.GetComponent<MenuSelectable>().SquarePosition());
    }

    IEnumerator HandleMenuSelectDelay(MenuState NextState, float delay, AudioClip sound)
    {
        _audioSource.clip = sound;
        if (NextState == MenuState.StartGame)
        {
            _audioSource.clip = _sfxMenuGameStart;
            delay = 4f;
            _whiteScreenAlpha = 0f;
            _whiteFadeInScreen.gameObject.SetActive(true);
            IsPreparingGame = true;
        }
        _audioSource.Play();
        yield return new WaitForSeconds(delay);
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
                break;
            }
            case MenuState.Credits:
            {
                _whiteFadeInScreen.gameObject.SetActive(true);
                _whiteScreenAlpha = 0f;
                break;
            }
            case MenuState.Exit:
            {
                break;
            }
            case MenuState.Main:
            {
                break;
            }
        }
        _creditsPanel.SetActive(CurrentState == MenuState.Credits);
        //_inPlayPanel.SetActive(CurrentState == GameState.InPlay);
    }

}
