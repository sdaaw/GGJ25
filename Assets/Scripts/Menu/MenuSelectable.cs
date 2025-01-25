using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
public class MenuSelectable : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Image selectionSquare;

    [SerializeField]
    private Image _selectionUnderline;

    [SerializeField]
    private TMP_Text _text, _mirroredText;

    public bool IsSelected;

    private bool _isDoingAnimation;

    private RectTransform _squareRect, _underlineRect, _textRect, _mirroredTextRect;

    private Vector2 _originalTextPosition, _originalMirroredTextPosition;

    private float xsway, ysway, xtime, ytime;

    public Action menuElementFunction;

    public MenuControls.MenuState MenuState;

    private void Awake()
    {
        IsSelected = false;
    }
    void Start()
    {
        _squareRect = selectionSquare.GetComponent<RectTransform>();
        _underlineRect = _selectionUnderline.GetComponent<RectTransform>();
        _textRect = _text.gameObject.GetComponent<RectTransform>();
        _mirroredTextRect = _mirroredText.gameObject.GetComponent<RectTransform>();
        selectionSquare.color = MenuControls.Instance.unselectedColor;
        _selectionUnderline.color = MenuControls.Instance.unselectedColor;
        _text.color = MenuControls.Instance.unselectedColor;
        _originalTextPosition = _textRect.anchoredPosition;
        _originalMirroredTextPosition = _mirroredTextRect.anchoredPosition;
        _mirroredText.color = new Color(_mirroredText.color.r, _mirroredText.color.g, _mirroredText.color.b, 0f);
        xsway = MenuControls.Instance.xsway;
        ysway = MenuControls.Instance.ysway;
        xtime = MenuControls.Instance.xtime;
        ytime = MenuControls.Instance.ytime;
    }

    public Vector2 SquarePosition()
    {
        return _squareRect.anchoredPosition + GetComponent<RectTransform>().anchoredPosition;
    } 

    //3F3F3F
    //BCBCBC deselected color

    // Update is called once per frame
    void Update()
    {
        if(_isDoingAnimation) 
        {
            DoFade();
        }
        if(IsSelected)
        {
            //text sway
            float x = Mathf.Sin(Time.time * xtime) * xsway * Time.deltaTime;
            float y = Mathf.Cos(Time.time * ytime) * ysway * Time.deltaTime;
            _textRect.anchoredPosition = new Vector2(_textRect.anchoredPosition.x + x, _textRect.anchoredPosition.y + y);
            _mirroredTextRect.anchoredPosition = new Vector2(_mirroredTextRect.anchoredPosition.x - x, _mirroredTextRect.anchoredPosition.y - y);
        } else
        {
            _textRect.anchoredPosition = _originalTextPosition;
            _mirroredTextRect.anchoredPosition = _originalMirroredTextPosition;
        }
    }

    public void ToggleSelect()
    {
        IsSelected = !IsSelected;
        _isDoingAnimation = true;
    }

    private void DoFade()
    {
        float speed = MenuControls.Instance.elementAnimSpeed;
        if (IsSelected)
        {
            selectionSquare.color = Color.Lerp(_text.color, MenuControls.Instance.selectedColor, speed * Time.deltaTime);
            _selectionUnderline.color = Color.Lerp(_text.color, MenuControls.Instance.selectedColor, speed * Time.deltaTime);
            _text.color = Color.Lerp(_text.color, MenuControls.Instance.selectedColor, speed * Time.deltaTime);
            _mirroredText.color = Color.Lerp(new Color(_mirroredText.color.r, _mirroredText.color.g, _mirroredText.color.b, 1f), new Color(_mirroredText.color.r, _mirroredText.color.g, _mirroredText.color.b, 0f), speed * Time.deltaTime);
            if (_text.color == MenuControls.Instance.selectedColor)
            {
                _isDoingAnimation = false;
            }
        }
        else
        {
            selectionSquare.color = Color.Lerp(_text.color, MenuControls.Instance.unselectedColor, speed * Time.deltaTime);
            _selectionUnderline.color = Color.Lerp(_text.color, MenuControls.Instance.unselectedColor, speed * Time.deltaTime);
            _text.color = Color.Lerp(_text.color, MenuControls.Instance.unselectedColor, speed * Time.deltaTime);
            _mirroredText.color = Color.Lerp(new Color(_mirroredText.color.r, _mirroredText.color.g, _mirroredText.color.b, 0f), new Color(_mirroredText.color.r, _mirroredText.color.g, _mirroredText.color.b, 1f), speed * Time.deltaTime);
            if (_text.color == MenuControls.Instance.unselectedColor)
            {
                _isDoingAnimation = false;
            }
        }
    }
}
