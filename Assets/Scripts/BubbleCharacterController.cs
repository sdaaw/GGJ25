using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;


public class BubbleCharacterController : Entity
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    private float _movementSpeed;

    [SerializeField]
    private float _mouseSensitivity, _cameraFov;


    [SerializeField]
    private float _cameraDistance;

    [SerializeField]
    private float _cameraDistanceMax = 50;

    private Camera _camera;

    private float mousey, mousex;

    private CharacterController _controller;

    private BubbleBehaviour _bubble;

    [SerializeField]
    private float _ycameraClampMin, _ycameraClampMax;

    private Color _originalColor;

    public bool IsInvulnerable;

    public LayerMask testLayer;

    [SerializeField]
    private float _gravity, _groundDistance;

    private bool _isGrounded;

    [SerializeField]
    private Transform _groundSensor;

    [SerializeField]
    private LayerMask _groundMask;

    [SerializeField]
    private Animator _faceAnimator;

    private float _takeDmgAnimTimer;
    private float _takeDmgAnimTimerMax = 2;

    private Vector3 _movement;

    private void Awake()
    {
        _bubble = GetComponent<BubbleBehaviour>();
    }
    void Start()
    {

        _camera = Camera.main;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        _controller = GetComponent<CharacterController>();
        //_camera.transform.SetParent(transform, false);

        GameManager.instance.HealthValueText.text = CurrentHealth.ToString();

    }

    // Update
    // is called once per frame
    void Update()
    {
        if(GameManager.instance != null && (GameManager.instance.IsPlayerFrozen || 
           GameManager.instance.StateHandler.CurrentState == GameStateHandler.GameState.Paused ||
           IsDead)) return;

        if (_takeDmgAnimTimer >= 0)
        {
            _takeDmgAnimTimer -= Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.P))
        {
            CurrentHealth += 0.5f;
        }

        HandleInput();
        HandleDamageCollider();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null) return;

        ContactPoint[] contacts = collision.contacts;

        //_bubble.HitBubble(transform.InverseTransformPoint(contacts[0].point));

        if (collision.collider.gameObject.GetComponent<Entity>())
        {
            var e = collision.collider.gameObject.GetComponent<Entity>();
            entitiesInsideCollider.Add(e);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision == null) return;

        ContactPoint[] contacts = collision.contacts;

        //_bubble.HitBubble(transform.InverseTransformPoint(contacts[0].point));

        if(collision.collider.gameObject.GetComponent<Entity>())
        {
            var e = collision.collider.gameObject.GetComponent<Entity>();
            if (entitiesInsideCollider.Contains(e))
            {
                entitiesInsideCollider.Remove(e);
            }
        }

    }

    [SerializeField]
    private float _damageTickTimer = 0.5f;
    private float _timer;
    public List<Entity> entitiesInsideCollider = new List<Entity>();

    [SerializeField]
    private float _tickDamage;

    protected void HandleDamageCollider()
    {

        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        } 
        else
        {
            _timer = _damageTickTimer;
            // tick dmg on all colliders inside
            DoDamageToColliders();
        }
    }

    protected void DoDamageToColliders()
    {
        for (int i = entitiesInsideCollider.Count - 1; i >= 0; i--)
        {
            var entity = entitiesInsideCollider[i];
            if (entity.type != EntityType.Player)
            {
                if (entity.entityConsumeThreshold <= CurrentHealth)
                {
                    var tickDmg = _tickDamage + (CurrentHealth / 100 * 2);


                    if ((entity.CurrentHealth - tickDmg) <= 0)
                    {
                        // increase size + hp
                        CurrentHealth += entity.scoreAmount;

                        entitiesInsideCollider.Remove(entity);
                    }

                    entity.CurrentHealth -= tickDmg;
                }
            }
        }
    }



    protected override void OnHealthChanged(float amount)
    {
        base.OnHealthChanged(amount);


        if (IsDead) return;
        GameManager.instance.HealthValueText.text = CurrentHealth.ToString();

        if(CurrentHealth < 5f)
        {
            GameManager.instance.ChatBoxController.RandomLowHealthWarning();
        }
        if (GameManager.instance != null && GameManager.instance.HealthValueText)
        {
            GameManager.instance.HealthValueText.text = CurrentHealth.ToString();
        }
        
        if (amount > 0)
        {
            SoundManager.PlayASource("Eating");
            if (CurrentHealth <= 750)
            {
                IncreaseSize(amount);
            }
        }
        else
        {
            BubbleTakeDamage(amount);  
        }
    }

    protected void BubbleTakeDamage(float amount)
    {
        _bubble.BubbleSize += amount;
        transform.localScale += new Vector3(1, 1, 1) * (amount / 10);

        if (_takeDmgAnimTimer <= 0)
        {
            _takeDmgAnimTimer = _takeDmgAnimTimerMax;
            StartCoroutine(DamageVisual());
            _faceAnimator.SetTrigger("DamageTrigger");
        } 
    }

    protected void IncreaseSize(float amount)
    {
        _bubble.BubbleSize += amount;
        transform.localScale += new Vector3(1, 1, 1) * (amount / 10);
        // _cameraDistance += amount;
    }

    IEnumerator DamageVisual()
    {
        _originalColor = _bubble.BubbleColorTint;
        float visualIntensity = 4f;
        _bubble.BubbleColorTint = _bubble.damagedColorTint;
        _bubble.DisplacementPower *= visualIntensity;
        _bubble.DisplacementSpeed *= visualIntensity;
        _bubble.OuterGlowWidth -= 1;
        yield return new WaitForSeconds(0.5f);
        _bubble.DisplacementPower /= visualIntensity;
        _bubble.DisplacementSpeed /= visualIntensity;
        _bubble.OuterGlowWidth += 1;
        _bubble.BubbleColorTint = _originalColor;
        IsInvulnerable = false;
    }
    void LateUpdate()
    {

        MouseCameraRotation();
    }


    private void MouseCameraRotation()
    {
        mousex += Input.GetAxis("Mouse X") * _mouseSensitivity * Time.fixedUnscaledDeltaTime;
        mousey -= Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.fixedUnscaledDeltaTime;

        mousey = Mathf.Clamp(mousey, _ycameraClampMin, _ycameraClampMax);

        Vector3 dir = new Vector3(0, 0, -Mathf.Clamp(CurrentHealth, 8, _cameraDistanceMax));
        Quaternion rotation = Quaternion.Euler(mousey, mousex, 0);
        _camera.transform.position = transform.position + rotation * dir;
        _camera.transform.LookAt(transform.position);
    }

    private bool walkSoundPlaying = false;

    [SerializeField]
    private AudioSource _walkSound;

    private void HandleInput()
    {
        _groundDistance = transform.localScale.x;
        _isGrounded = Physics.CheckSphere(_groundSensor.transform.position, _groundDistance, _groundMask);
        if (_isGrounded)
        {
            float x = Input.GetAxis("Horizontal") * _movementSpeed * Time.deltaTime;
            float y = Input.GetAxis("Vertical") * _movementSpeed * Time.deltaTime;


            _movement = _camera.transform.right * x + _camera.transform.forward * y;
        }
        _movement.y = 0f;

        _movement.y += _gravity;

        _controller.Move(_movement);
        if (_movement.magnitude != 0f)
        {
            if (!_isGrounded) return;
            _bubble.IsMoving = true;
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime);

            Quaternion CamRotation = _camera.transform.rotation;
            CamRotation.x = 0f;
            CamRotation.z = 0f;

            transform.rotation = Quaternion.Lerp(transform.rotation, CamRotation, 0.1f);

        } 
        else
        {
            _bubble.IsMoving = false;
            
        }
        // Debug.Log(_movement.magnitude);
        if (_movement.magnitude > 4)
        {
            if (!_walkSound.isPlaying)
            {
                _walkSound.Play();
                walkSoundPlaying = true;
            }
        }
        else if (walkSoundPlaying && _movement.magnitude <= 4)
        {
            _walkSound.Stop();
            walkSoundPlaying = false;
        }
    }
}
