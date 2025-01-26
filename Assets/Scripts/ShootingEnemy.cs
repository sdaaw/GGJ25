using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootingEnemy : Enemy
{
    [SerializeField]
    private GameObject _muzzleFlash;

    [SerializeField]
    private GameObject _bullet;

    [SerializeField]
    private float _bulletVelocity;

    [SerializeField]
    private float _damage;

    private float _shootTimer;

    [SerializeField]
    private float _shootTimerMax;

    [SerializeField]
    private string _shootSound = "";

    // [SerializeField]
    // private float _FOV = 90;

    [SerializeField]
    protected NavMeshAgent Agent;

    protected override void Awake()
    {
        base.Awake();
        Agent = GetComponent<NavMeshAgent>();
    }

    protected override void Update()
    {
        base.Update();

        ShootLogic();

        if (_animator != null)
        {
            _animator.SetFloat("Speed", Agent.velocity.magnitude);
        }

        //TODO: need to change this, so enemies can turn around while moving to show samuels art
        // transform.LookAt(target);
        }

    public void ShootLogic()
    {
        CheckLoSToPlayer();

        _shootTimer += Time.deltaTime;

        if (_shootTimer >= _shootTimerMax)
        {
            if (!chaseTarget)
            {
                Shoot();
                _shootTimer = 0;
            }
        }
    }

    public void Shoot()
    {
        GameObject bullet = Instantiate(_bullet, transform.position + transform.forward, Quaternion.identity);
        bullet.GetComponent<Bullet>().Activate(_bulletVelocity, transform.forward, transform, _damage);

        if (_animator != null)
        {
            _animator.SetTrigger("Shoot");
        }


        if (_shootSound != "")
        {
            SoundManager.PlayASource(_shootSound);
        }

        if (_muzzleFlash != null)
        {
            StartCoroutine(muzzleFlash());
        }
    }

    public void CheckLoSToPlayer()
    {
        RaycastHit hit;
        if (target == null) return;
        var rayDirection = target.position - transform.position;

        if (Physics.Raycast(transform.position - new Vector3(0, 0.5f, 0), rayDirection, out hit))
        {
            if (hit.transform.GetComponent<BubbleCharacterController>())
            {
                chaseTarget = false;
            }
            else
            {
                chaseTarget = true;
            }
        }

    }

    public void CheckPlayerAgroRange()
    {

    }

    IEnumerator muzzleFlash()
    {
        _muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        _muzzleFlash.SetActive(false);
    }


    /*public int ColliderArraySortComparer(Collider A, Collider B)
    {
        if (A == null && B != null)
        {
            return 1;
        }
        else if (A != null && B == null)
        {
            return -1;
        }
        else if (A == null && B == null)
        {
            return 0;
        }
        else
        {
            return Vector3.Distance(Agent.transform.position, A.transform.position).CompareTo(Vector3.Distance(Agent.transform.position, B.transform.position));
        }
    }*/
}
