using System.Collections;
using TMPro;
using UnityEngine;

public class UITextBehaviours : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private readonly string[] GLITCH_LETTERS = new string[] { "?", "#", "@", "&" };

    private TMP_Text _text;

    private string _myTextContent;

    [SerializeField]
    private float _textScrollSpeed;

    [SerializeField]
    private bool IsScrollingText, IsGlitching;
    void Start()
    {
        _text = GetComponent<TMP_Text>();
        _myTextContent = _text.text;
        if(IsScrollingText)
        {
            _text.text = "";
            ScrollText(_myTextContent, _textScrollSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ScrollText(string text, float speed)
    {
        StartCoroutine(ScrollingText(text, speed));
    }

    IEnumerator ScrollingText(string text, float speed)
    {
        for(int i = 0; i < text.Length; i++)
        {
            _text.text += text[i];
            yield return new WaitForSeconds(speed);
        }
    }

    IEnumerator GlitchEffect()
    {
        
        yield return new WaitForSeconds(0.2f);
    }
}
