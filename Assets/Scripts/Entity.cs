using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    protected virtual void Start()
    {
        _renderer = GetComponent<Renderer>();
        _animator = GetComponent<Animator>();
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
        SoundManager.PlayASource("Death");
        if (_animator != null && hasDeathAnim)
        {
            _animator.SetTrigger("Death");
        }

        var econ = FindFirstObjectByType<EnemyController>();
        var enemy = this.GetComponent<Enemy>();

        if (econ.currentWave.currentWaveEnemies.Contains(enemy))
        {
            econ.currentWave.currentWaveEnemies.Remove(enemy);
        }

        if (hasDeathAnim)
        {
            StartCoroutine(WaitDeath());
        }
        else if (GetComponent<BubbleCharacterController>())
        {
            Cursor.lockState = CursorLockMode.None;
            // TODO: player died here

            // GameManager.instance.StateHandler.CurrentState = GameStateHandler.GameState.PlayerDeath;
            StartCoroutine(ReturnToMainMenuOnDeath());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator ReturnToMainMenuOnDeath()
    {
        yield return new WaitForSeconds(3);
        Application.LoadLevel("MainMenu");
    }

    private IEnumerator WaitDeath()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    private IEnumerator DamageVisual()
    {
        if (_renderer == null || type == EntityType.Player) yield break;
        _dmgCoroutinePlaying = true;

        _renderer.material.color *= 2;
        yield return new WaitForSeconds(0.2f);
        _renderer.material.color /= 2;
        _dmgCoroutinePlaying = false;
    }
}
