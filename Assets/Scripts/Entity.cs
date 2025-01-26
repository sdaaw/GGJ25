using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Entity : MonoBehaviour
{
    public enum EntityType
    {
        None,
        Normal,
        Boss,
        Player
    }

    public EntityType type;
    public float CurrentHealth
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            var change = (_currentHealth - value) * (-1);
            // Debug.Log(change);
            _currentHealth = value;
            OnHealthChanged(change);
        }
    }

    [SerializeField]
    private float _currentHealth;

    private Renderer _renderer;

    public bool hasDeathAnim;
    [SerializeField]
    protected Animator _animator;

    private bool _dmgCoroutinePlaying = false;

    public float scoreAmount = 0;

    public float entityConsumeThreshold = 0;

    public EnemyWave waveOwner = null;

    [SerializeField]
    private GameObject _deathDebris;

    public bool IsDead;


    private float xsway, xtime, ztime, zsway;

    private bool _isDying;

    [SerializeField]
    private string[] _deathSounds;

    [SerializeField]
    private bool _isFinalTownBuilding = false;

    protected virtual void Start()
    {
        _renderer = GetComponent<Renderer>();
        _animator = GetComponentInChildren<Animator>();
        xsway = 4f;
        xtime = 0.2f;
        ztime = 0.5f;
        zsway = 2f;
    }

    void Update()
    {
    }

    protected virtual void OnHealthChanged(float amount)
    {
        if (amount < 0)
        {
            if(!_dmgCoroutinePlaying)
            {
                StartCoroutine(DamageVisual());
            }
        }

        if (_currentHealth <= 0)
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.EntitiesInWorld.Remove(gameObject);
            }
            OnDie();
        }
    }

    public void OnDie()
    {
        if (IsDead) return;

        IsDead = true;
        if (_deathSounds.Length > 0)
        {
            SoundManager.PlayASource(_deathSounds[Random.Range(0,_deathSounds.Length)]);
        }
        if (_animator != null && hasDeathAnim)
        {
            // _animator.SetTrigger("Death");
        }

        var econ = FindFirstObjectByType<EnemyController>();
        var enemy = this.GetComponent<Enemy>();

        if (waveOwner != null && waveOwner.currentWaveEnemies.Contains(enemy))
        {
            waveOwner.currentWaveEnemies.Remove(enemy);
        }
        
        if (hasDeathAnim)
        {
            if (_isDying) return;
            _isDying = true;
            StartCoroutine(WaitDeath());
        }
        else if (GetComponent<BubbleCharacterController>())
        {
            Cursor.lockState = CursorLockMode.None;
            // TODO: player died here

            GameManager.instance.StateHandler.CurrentState = GameStateHandler.GameState.Death;
            //StartCoroutine(ReturnToMainMenuOnDeath());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator ReturnToMainMenuOnDeath()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("MenuScene");
    }

    private IEnumerator WaitDeath()
    {
        for(int i = 0; i < 10; i++)
        {
            Vector3 pos = new Vector3(transform.position.x + Random.Range(-5, 5), transform.position.y + Random.Range(1f, 5f), transform.position.z + Random.Range(-5, 5));
            GameObject a = Instantiate(_deathDebris, pos, Quaternion.identity);
            a.GetComponent<DeathDebrisBehaviour>().startPos = pos;
            a.GetComponent<DeathDebrisBehaviour>().GeneratePoints();
            yield return new WaitForSeconds(0.05f);
        }

        if (_isFinalTownBuilding && FindFirstObjectByType<TownResources>())
        {
            var town = FindFirstObjectByType<TownResources>();
            town.townBuildings.Remove(this.gameObject);
        }

        Destroy(gameObject);
    }

    private IEnumerator DamageVisual()
    {
        if (_renderer == null || type == EntityType.Player) yield break;
        _dmgCoroutinePlaying = true;

        _renderer.material.color *= 2;
        yield return new WaitForSeconds(0.2f);
        _renderer.material.color /= 2;
        if(_deathDebris)
        {
            for(int i = 0; i < 4; i++)
            {
                GameObject a = Instantiate(_deathDebris, transform.position, Quaternion.identity);
                a.GetComponent<DeathDebrisBehaviour>().startPos = transform.position;
                a.GetComponent<DeathDebrisBehaviour>().GeneratePoints();
            } 
        }
        _dmgCoroutinePlaying = false;
    }
}
