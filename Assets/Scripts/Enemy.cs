using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : Entity
{
    [SerializeField]
    protected Transform target;

    [SerializeField]
    private Vector3 _targetPosition;

    [SerializeField]
    protected bool _isEnabled = true;

    public bool chaseTarget = true;

    private NavMeshAgent _agent;
    private float timer;

    [SerializeField]
    private float _timerMax;

    private Slider healthBar;

    protected BubbleCharacterController _player;

    [SerializeField]
    private float _healthBarRange = 30;

    private Camera _cam;

    private float _maxHealth;

    private Color _barStartColor;

    public float agroRange = 100f;

    protected virtual void Awake()
    {
        _agent = this.GetComponent<NavMeshAgent>();
        _player = FindFirstObjectByType<BubbleCharacterController>();
        _cam = Camera.main;
        _maxHealth = CurrentHealth;

        healthBar = GetComponentInChildren<Slider>();

        if (healthBar)
            _barStartColor = healthBar.colors.normalColor;
    }

    protected virtual void Update()
    {
        if (_isEnabled)
        {
            if (chaseTarget && Vector3.Distance(_player.transform.position, transform.position) <= agroRange)
            {
                if (_agent.isOnNavMesh)
                {
                    _agent.SetDestination(target.position);
                }
            }
            else
            {
                MoveAround();
                if (_agent.isOnNavMesh)
                {
                    _agent.SetDestination(_targetPosition);
                }   
            }

            if (target == null) return;
            transform.LookAt(new Vector3(target.position.x, target.position.y, target.position.z));
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

            UpdateHealthBar();
        }
    }

    protected virtual void MoveAround()
    {
        timer += Time.deltaTime;

        if (timer >= _timerMax)
        {
            GetRandomClosePosition();
            timer = 0;
        }
    }

    public virtual void SetTarget(Transform target)
    {
        this.target = target;
    }

    public virtual void SetTarget(Vector3 target)
    {
        _targetPosition = target;
    }

    protected virtual void GetRandomClosePosition()
    {
        Vector3 target = RandomNavSphere(transform.position, 3, -1);
        SetTarget(target);
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    private void UpdateHealthBar()
    {
        if (!healthBar)
            return;


        if (_player && Vector3.Distance(transform.position, _player.transform.position) >= _healthBarRange)
        {
            if (healthBar.gameObject.activeSelf)
                healthBar.gameObject.SetActive(false);
        }
        else
        {
            if (!healthBar.gameObject.activeSelf)
                healthBar.gameObject.SetActive(true);
        }


        healthBar.value = CurrentHealth / _maxHealth;
        healthBar.transform.LookAt(healthBar.transform.position + _cam.transform.rotation * Vector3.back,
                                       _cam.transform.rotation * Vector3.down);
        /*float dist = Vector3.Distance(Camera.main.transform.position, healthBar.transform.position) * 0.025f;
        healthBar.transform.localScale = Vector3.one * dist;*/
    }

    public void FlashHealthBar()
    {
        if (!healthBar)
            return;

        Image im = healthBar.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        StartCoroutine(FlashHealthBar(im, 0.1f));
    }

    private IEnumerator FlashHealthBar(Image im, float dur)
    {
        if (!healthBar.transform.parent.gameObject.activeSelf)
            healthBar.transform.parent.gameObject.SetActive(true);
        im.color = Color.white;
        yield return new WaitForSeconds(dur);
        im.color = _barStartColor;
    }
}
