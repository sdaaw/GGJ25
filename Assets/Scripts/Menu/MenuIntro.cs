using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
public class MenuIntro : MonoBehaviour
{

    [SerializeField]
    private Image _characterArt, _screenFadeImage, _gameLogo, _fgjLogo;

    private AudioSource _bgmAudioSource;

    [SerializeField]
    private AudioClip _bgm, _bgmIntro;

    [SerializeField]
    private TMP_Text _logoText;

    private float _timer;

    private bool _isIntroFade, _gameActive;

    [SerializeField]
    private float _fadeSpeed;

    private float _characterAlpha, _fadeScreenAlpha;

    [SerializeField]
    private GameObject _pointerBall;
    private RectTransform _pointerBallRect;


    [SerializeField]
    private float _pointerBallSpeed;

    [SerializeField]
    private int _pointerBallCurvePrecision;


    [SerializeField]
    private RectTransform[] _curvePoints;
    void Start()
    {
        _pointerBallRect = _pointerBall.GetComponent<RectTransform>();
        _pointerBall.GetComponent<PointerBall>().isIntroPhase = true;

        _bgmAudioSource = GetComponent<AudioSource>();
        _gameActive = true;
        _bgmAudioSource.clip = _bgm;
        _bgmAudioSource.Play();

        _bgmAudioSource.volume = 0.7f;
        _characterArt.color = new Color(_characterArt.color.r, _characterArt.color.g, _characterArt.color.b, 0f);
        _gameLogo.color = new Color(_gameLogo.color.r, _gameLogo.color.g, _gameLogo.color.b, 0f);
        _isIntroFade = true;
        _fadeScreenAlpha = 1f;
    }


    void Update()
    {

        if(!_gameActive)
        {
            return;
        }

        if(_isIntroFade)
        {
            if(_fadeScreenAlpha <= 0f)
            {
                _isIntroFade = false;
                _screenFadeImage.gameObject.SetActive(false);
                return;
            }
            _fadeScreenAlpha -= _fadeSpeed * Time.deltaTime;
            _logoText.color = new Color(_logoText.color.r, _logoText.color.g, _logoText.color.b, _fadeScreenAlpha);
            _fgjLogo.color = new Color(_fgjLogo.color.r, _fgjLogo.color.g, _fgjLogo.color.b, _fadeScreenAlpha);
            _screenFadeImage.color = new Color(_screenFadeImage.color.r, _screenFadeImage.color.g, _screenFadeImage.color.b, _fadeScreenAlpha);
        } else
        {
            FadeCharacterArtIn();
        }

        if(MenuControls.Instance.IsPreparingGame)
        {
            _bgmAudioSource.volume -= 0.5f * Time.deltaTime;
        }
    }

    private void FadeCharacterArtIn()
    {
        if (_characterAlpha >= 0.7f)
        {
            return;
        }
        if (_pointerBall.GetComponent<PointerBall>().isIntroPhase)
        {
            StartCoroutine(InterpolateBezier(_pointerBallCurvePrecision, _pointerBallRect, _pointerBallSpeed));
            _pointerBall.GetComponent<PointerBall>().isIntroPhase = false;
        }
        _characterAlpha += 0.1f * Time.deltaTime;
        _characterArt.color = new Color(_characterArt.color.r, _characterArt.color.g, _characterArt.color.b, _characterAlpha);
        _gameLogo.color = new Color(_gameLogo.color.r, _gameLogo.color.g, _gameLogo.color.b, _characterAlpha);
    }



    IEnumerator InterpolateBezier(int precision, RectTransform target, float speed)
    {
        for (int i = 0; i <= precision; i++)
        {
            float t = i / (float)precision;
            target.anchoredPosition = CubicBezierEval(
                _curvePoints[0].anchoredPosition,
                _curvePoints[1].anchoredPosition,
                _curvePoints[2].anchoredPosition,
                _curvePoints[3].anchoredPosition, t);
            yield return new WaitForSeconds(speed);
        }
        _pointerBall.GetComponent<PointerBall>().isIntroPhase = false;
    }

    public static Vector2 CubicBezierEval(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float u = 1 - t;
        return u * u * u * p0 + 3 * u * u * t * p1 + 3 * u * t * t * p2 + t * t * t * p3;
    }
}
