using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuCreditsNameElement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    [SerializeField]
    private Image _selectionUnderline;

    [SerializeField]
    private TMP_Text _text, _mirroredText;

    public bool IsSelected;

    private bool _isDoingAnimation;

    private RectTransform _textRect, _mirroredTextRect;

    private Vector2 _originalTextPosition, _originalMirroredTextPosition;

    private float xsway, ysway, xtime, ytime;


    private void Awake()
    {
        IsSelected = false;
    }
    void Start()
    {
        _textRect = _text.gameObject.GetComponent<RectTransform>();
        _mirroredTextRect = _mirroredText.gameObject.GetComponent<RectTransform>();
        _text.color = MenuControls.Instance.unselectedColor;
        _originalTextPosition = _textRect.anchoredPosition;
        _originalMirroredTextPosition = _mirroredTextRect.anchoredPosition;
        _mirroredText.color = new Color(_mirroredText.color.r, _mirroredText.color.g, _mirroredText.color.b, 0f);
        xsway = MenuControls.Instance.xsway;
        ysway = MenuControls.Instance.ysway;
        xtime = MenuControls.Instance.xtime;
        ytime = MenuControls.Instance.ytime;
    }

    void Update()
    {
        if (_isDoingAnimation)
        {
            DoFade();
        }
        if (IsSelected)
        {
            //text sway
            float x = Mathf.Sin(Time.time * xtime) * xsway * Time.deltaTime;
            float y = Mathf.Cos(Time.time * ytime) * ysway * Time.deltaTime;
            _textRect.anchoredPosition = new Vector2(_textRect.anchoredPosition.x + x, _textRect.anchoredPosition.y + y);
            _mirroredTextRect.anchoredPosition = new Vector2(_mirroredTextRect.anchoredPosition.x - x, _mirroredTextRect.anchoredPosition.y - y);
        }
        else
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

    public void DoFade()
    {
        float speed = MenuControls.Instance.elementAnimSpeed;

        if (IsSelected)
        {
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
