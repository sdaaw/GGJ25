using TMPro;
using UnityEngine;

public class PointerBall : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    public float xsway, xtime, ysway, ytime;

    [SerializeField]
    public float bxsway, bxtime, bysway, bytime;

    [SerializeField]
    private GameObject _innerBall;

    private RectTransform _rt, _innerRt;

    private bool _isMoving;
    private Vector2 _destination, _startPos;

    [SerializeField]
    private float _curveAmount;

    private float _elapsedTime;
    [SerializeField]
    private float _pointerSpeed;

    private bool _curveSide;

    public bool isIntroPhase;

    void Start()
    {
        _rt = GetComponent<RectTransform>();
        _innerRt = _innerBall.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isIntroPhase) return;
        float bx = Mathf.Sin(Time.time * bxtime) * bxsway * Time.deltaTime;
        float by = Mathf.Cos(Time.time * bytime) * bysway * Time.deltaTime;
        _innerRt.anchoredPosition = new Vector2(_innerRt.anchoredPosition.x + bx, _innerRt.anchoredPosition.y + by);

        if (_isMoving)
        {
            _elapsedTime += 1 * Time.deltaTime;
            float t = _elapsedTime / _pointerSpeed;
            if(t > 1f)
            {
                t = 1f;
                _isMoving = false;
            }

            float cx = _curveSide ? Mathf.Lerp(_startPos.x, _destination.x, t) + _curveAmount * Mathf.Sin(Mathf.PI * t)
                                  : Mathf.Lerp(_startPos.x, _destination.x, t) - _curveAmount * Mathf.Sin(Mathf.PI * t);

            float cy = Mathf.Lerp(_startPos.y , _destination.y, t);

            _rt.anchoredPosition = new Vector2(cx, cy);
            return;
        }

        float x = Mathf.Sin(Time.time * xtime) * xsway * Time.deltaTime;
        float y = Mathf.Cos(Time.time * ytime) * ysway * Time.deltaTime;
        _rt.anchoredPosition = new Vector2(_rt.anchoredPosition.x + x, _rt.anchoredPosition.y + y);
    }



    public void MoveSelectionBall(Vector2 position)
    {
        _curveSide = !_curveSide;
        _destination = position;
        _startPos = _rt.anchoredPosition;
        _elapsedTime = 0f;
        _isMoving = true;
    }
}

