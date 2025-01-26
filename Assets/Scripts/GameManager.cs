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

    void Start()
    {
        ChatBoxController = GetComponent<UIChatBoxController>();
        StateHandler = GetComponent<GameStateHandler>();
        if (instance == null) { instance = this; }
        player = Instantiate(_playerPrefab, SpawnPoint.position, Quaternion.identity);
        _whiteFadeAlpha = _whiteFadeImage.color.a;
        //StateHandler.CurrentState = GameStateHandler.GameState.StartScene;
        StateHandler.CurrentState = GameStateHandler.GameState.InPlay;
    }

    private void Update()
    {
        if(StateHandler.CurrentState == GameStateHandler.GameState.StartScene)
        {
            _whiteFadeAlpha -= 0.1f * Time.deltaTime;
            _whiteFadeImage.color = new Color(_whiteFadeImage.color.r, _whiteFadeImage.color.g, _whiteFadeImage.color.b, _whiteFadeAlpha);
            if(_whiteFadeAlpha <= 0)
            {
                StateHandler.CurrentState = GameStateHandler.GameState.InPlay;
            }
        }
    }

}
