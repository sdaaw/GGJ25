using System.Collections.Generic;
using UnityEngine;

public class MenuControls : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    [SerializeField]
    private List<GameObject> _selectableMenuElements;

    private int _selectionIndex;

    [SerializeField]
    private GameObject _selectorBall;

    private GameObject _selectedMenuObject;

    public Color selectedColor, unselectedColor;

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _sfxMenuNavigate;

    public static MenuControls Instance;

    [SerializeField]
    public float xsway, xtime, ysway, ytime;

    public float elementAnimSpeed;

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
            _audioSource.Play();
            _selectionIndex++;
            if (_selectionIndex > _selectableMenuElements.Count - 1)
            {
                _selectionIndex = 0;
            }
            print(_selectionIndex);
            HandleSelection();
        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            _audioSource.Play();
            _selectionIndex--;
            if (_selectionIndex < 0)
            {
                _selectionIndex = _selectableMenuElements.Count - 1;
            }
            print(_selectionIndex);
            HandleSelection();
        }

        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            //confirm select
        }
    }

    private void HandleSelection()
    {
        _selectedMenuObject.GetComponent<MenuSelectable>().ToggleSelect(); //toggle old
        _selectedMenuObject = _selectableMenuElements[_selectionIndex];
        _selectedMenuObject.GetComponent<MenuSelectable>().ToggleSelect(); //toggle new
        _selectorBall.GetComponent<PointerBall>().MoveSelectionBall(_selectedMenuObject.GetComponent<MenuSelectable>().SquarePosition());
    }

}
