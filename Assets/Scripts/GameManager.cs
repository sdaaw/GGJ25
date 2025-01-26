using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject _playerPrefab;

    public TMP_Text HealthValueText;

    [HideInInspector]
    public GameObject player;

    public TMP_Text _gameIntroText;

    [SerializeField]
    private Image _whiteFadeImage;

    [SerializeField]
    private float _whiteFadeOutSpeed;

    public static GameManager instance;

    public Transform SpawnPoint;

    public List<GameObject> EntitiesInWorld = new List<GameObject>();

    public GameStateHandler StateHandler;

    public UIChatBoxController ChatBoxController;

    public bool IsPlayerFrozen;

    private float _whiteFadeAlpha;

    private bool _introDone;

    [SerializeField]
    private bool SKIP_INTRO;

    void Start()
    {
        ChatBoxController = GetComponent<UIChatBoxController>();
        StateHandler = GetComponent<GameStateHandler>();
        if (instance == null) { instance = this; }
        player = Instantiate(_playerPrefab, SpawnPoint.position, Quaternion.identity);
        if(SKIP_INTRO)
        {
            StateHandler.CurrentState = GameStateHandler.GameState.InPlay;
            _whiteFadeImage.gameObject.SetActive(false);
            return;
        }
        _whiteFadeAlpha = _whiteFadeImage.color.a;
        StateHandler.CurrentState = GameStateHandler.GameState.StartScene;
        StartCoroutine(IntroTextVisual());
    }

    private void Update()
    {
        if(StateHandler.CurrentState == GameStateHandler.GameState.StartScene)
        {
            if (!_introDone) return;
            _whiteFadeAlpha -= 0.1f * Time.deltaTime;
            _whiteFadeImage.color = new Color(_whiteFadeImage.color.r, _whiteFadeImage.color.g, _whiteFadeImage.color.b, _whiteFadeAlpha);
            if(_whiteFadeAlpha <= 0)
            {
                StateHandler.CurrentState = GameStateHandler.GameState.InPlay;
            }
        }
    }

    IEnumerator IntroTextVisual()
    {
        string[] script = new string[] { "I've been having these weird thoughts lately..", "like is any of this real or not ?" };
        for (int i = 0; i < script[0].Length; i++)
        {
            _gameIntroText.text += script[0][i];
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(3f);
        for (int i = _gameIntroText.text.Length; i > 0; i--)
        {
            _gameIntroText.text = _gameIntroText.text.Remove(i - 1);
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < script[1].Length; i++)
        {
            _gameIntroText.text += script[1][i];
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(3f);
        for (int i = _gameIntroText.text.Length; i > 0; i--)
        {
            _gameIntroText.text = _gameIntroText.text.Remove(i - 1);
            yield return new WaitForSeconds(0.03f);
        }
        _introDone = true;
    }

}
