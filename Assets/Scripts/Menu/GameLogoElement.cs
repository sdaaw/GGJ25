using UnityEngine;

public class GameLogoElement : MonoBehaviour
{
    [SerializeField]
    public float xsway, xtime, ysway, ytime;

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
    }
}
