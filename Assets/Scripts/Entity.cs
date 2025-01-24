using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{


    public enum EntityType
    {
        None,
        Normal,
        Boss
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
            _currentHealth = value;
            OnHealthChanged();
        }
    }

    [SerializeField]
    private float _currentHealth;

    private Renderer _renderer;

    public bool hasDeathAnim;
    [SerializeField]
    protected Animator _animator;

    protected virtual void Start()
    {
        _renderer = GetComponent<Renderer>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
    }

    protected void OnHealthChanged()
    {
        StartCoroutine(DamageVisual());
        if (_currentHealth <= 0)
        {
            GameManager.instance.EntitiesInWorld.Remove(gameObject);
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
        else if (GetComponent<FPSController>())
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
        if (_renderer == null) yield return null;

        _renderer.material.color *= 2;
        yield return new WaitForSeconds(0.2f);
        _renderer.material.color /= 2;
    }
}
