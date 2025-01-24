using UnityEngine;

public class FGJLogoElement : MonoBehaviour
{

    [SerializeField]
    private float xsway, xtime, ysway, ytime;

    [SerializeField]
    private float _xrotMultiplier, _yrotMultiplier;

    private RectTransform _rt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Mathf.Sin(Time.time * xtime) * xsway * Time.deltaTime;
        float y = Mathf.Cos(Time.time * ytime) * ysway * Time.deltaTime;
        _rt.anchoredPosition = new Vector2(_rt.anchoredPosition.x + x, _rt.anchoredPosition.y + y);
        _rt.eulerAngles = new Vector3(_rt.eulerAngles.x + (x * _xrotMultiplier), _rt.eulerAngles.y + (y * _yrotMultiplier));
    }
}
