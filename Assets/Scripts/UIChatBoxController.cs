using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIChatBoxController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private bool _isShowing, _in, _out;

    [SerializeField]
    private string[] LOW_HEALTH_WARNINGS;

    [SerializeField]
    private TMP_Text _text;

    [SerializeField]
    private Image _chatBox;

    private RectTransform _chatBoxRect;

    [SerializeField]
    private Vector2 _chatBoxStartPos;

    [SerializeField]
    private Vector2 _chatBoxEndPos;

    private float _elapsedTime;

    [SerializeField]
    private float _chatBoxLerpSpeed;
    void Start()
    {
        _chatBoxRect = _chatBox.GetComponent<RectTransform>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyUp(KeyCode.O))
        {
            ShowText("Okay hello :D", 5f);
        }


        if (!_isShowing) return;
        if(_in)
        {
            _elapsedTime += 1 * Time.deltaTime;
            float t = _elapsedTime / _chatBoxLerpSpeed;
            _chatBoxRect.anchoredPosition = Vector2.Lerp(_chatBoxStartPos, _chatBoxEndPos, t);
            if (t > 1f)
            {
                t = 1f;
            }
        }
        if(_out) 
        {
            _elapsedTime += 1 * Time.deltaTime;
            float t = _elapsedTime / _chatBoxLerpSpeed;
            _chatBoxRect.anchoredPosition = Vector2.Lerp(_chatBoxEndPos, _chatBoxStartPos, t);
            if (t > 1f)
            {
                t = 1f;
                _isShowing = false;
            }
        }
    }

    public void RandomLowHealthWarning()
    {
        if (_isShowing) return;

        ShowText(LOW_HEALTH_WARNINGS[Random.Range(0, LOW_HEALTH_WARNINGS.Length)], 4f);
    }

    public void ShowText(string text, float duration)
    {
        _out = false;
        _isShowing = true;
        _in = true;
        _elapsedTime = 0;
        _text.text = text;
        StartCoroutine(DoVisual(duration));
    }

    IEnumerator DoVisual(float duration)
    {
        yield return new WaitForSeconds(duration);
        _elapsedTime = 0;
        _out = true;
    }


}
