using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject _playerPrefab;

    [HideInInspector]
    public GameObject player;

    public static GameManager instance;

    public Transform SpawnPoint;

    public List<GameObject> EntitiesInWorld = new List<GameObject>();

    public GameStateHandler GameStateHandler;

    public bool IsPlayerFrozen;

    void Start()
    {
        GameStateHandler = GetComponent<GameStateHandler>();
        if (instance == null) { instance = this; }
        player = Instantiate(_playerPrefab, SpawnPoint.position, Quaternion.identity);
    }

}
