using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject _playerPrefab;

    public TMP_Text HealthValueText;

    [HideInInspector]
    public GameObject player;

    public TMP_Text _gameIntroText, _deathText;

    public Image whiteFadeImage;

    public static GameManager instance;

    public Transform SpawnPoint;

    public List<GameObject> EntitiesInWorld = new List<GameObject>();

    public GameStateHandler StateHandler;

    public UIChatBoxController ChatBoxController;

    public bool IsPlayerFrozen;

    private float _whiteFadeAlpha;

    private bool _introDone;

    public AudioSource playerAudioSource;

    public AudioClip deathAudioSfx;

    [SerializeField]
    private bool SKIP_INTRO;

    private bool _isDeathScene;

    void Start()
    {
        playerAudioSource = GetComponent<AudioSource>();
        ChatBoxController = GetComponent<UIChatBoxController>();
        StateHandler = GetComponent<GameStateHandler>();
        if (instance == null) { instance = this; }
        player = Instantiate(_playerPrefab, SpawnPoint.position, Quaternion.identity);
        if(SKIP_INTRO)
        {
            StateHandler.CurrentState = GameStateHandler.GameState.InPlay;
            whiteFadeImage.gameObject.SetActive(false);
            return;
        }
        _whiteFadeAlpha = whiteFadeImage.color.a;
        StateHandler.CurrentState = GameStateHandler.GameState.StartScene;
        StartCoroutine(IntroTextVisual());
    }

    private void Update()
    {
        if(StateHandler.CurrentState == GameStateHandler.GameState.StartScene)
        {
            if (!_introDone) return;
            _whiteFadeAlpha -= 0.1f * Time.deltaTime;
            whiteFadeImage.color = new Color(whiteFadeImage.color.r, whiteFadeImage.color.g, whiteFadeImage.color.b, _whiteFadeAlpha);
            if(_whiteFadeAlpha <= 0)
            {
                StateHandler.CurrentState = GameStateHandler.GameState.InPlay;
            }
        }

        if(StateHandler.CurrentState == GameStateHandler.GameState.Death)
        {
            HandleDeath();
        }
    }

    IEnumerator IntroTextVisual()
    {
        string[] script = new string[] { "Bubble must consume ...", " Consume enemies and buildings to grow" };
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

    private void HandleDeath()
    {
        _whiteFadeAlpha += 0.3f * Time.deltaTime;
        whiteFadeImage.color = new Color(whiteFadeImage.color.r, whiteFadeImage.color.g, whiteFadeImage.color.b, _whiteFadeAlpha);
        if(_whiteFadeAlpha >= 1)
        {
            _whiteFadeAlpha = 1f;
            _isDeathScene = false;
            if(StateHandler.CurrentState == GameStateHandler.GameState.Death)
            {
                StateHandler.CurrentState = GameStateHandler.GameState.DeathScreen;
                StartCoroutine(DeathMessageVisual());
            }
        }
    }

    IEnumerator DeathMessageVisual()
    {
        string[] script = new string[] { "Sorry, your bubble has burst .." };
        for (int i = 0; i < script[0].Length; i++)
        {
            _gameIntroText.text += script[0][i];
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MenuScene");
    }

}
