using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.SceneView;
using UnityEngine.InputSystem.XR;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.GraphicsBuffer;
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.EventSystems.EventTrigger;

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
    private Animator _faceAnimator;

    private float _takeDmgAnimTimer;
    private float _takeDmgAnimTimerMax = 2;

    private void Awake()
    {
        _bubble = GetComponent<BubbleBehaviour>();
    }
    void Start()
    {

        _camera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        _controller = GetComponent<CharacterController>();
        //_camera.transform.SetParent(transform, false);

    }

    // Update
    // is called once per frame
    void Update()
    {
        if(GameManager.instance != null && (GameManager.instance.IsPlayerFrozen || 
           GameManager.instance.StateHandler.CurrentState == GameStateHandler.GameState.Paused)) return;

        if (_takeDmgAnimTimer >= 0)
        {
            _takeDmgAnimTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _bubble.DisplacementPower -= 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            _bubble.DisplacementPower += 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            _bubble.DisplacementSpeed += 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            _bubble.DisplacementSpeed -= 0.1f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            /*
            if (IsInvulnerable) return;
            if (amount < 0)
            {
                IsInvulnerable = true;
            }  
            */
            _faceAnimator.SetTrigger("DamageTrigger");
            CurrentHealth -= 0.5f;
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
            CurrentHealth += 0.5f;
        }

        if(Input.GetMouseButton(0))
        {
            Ray r = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(r, out hit, Mathf.Infinity))
            {
                // Debug.LogWarning(hit.transform.gameObject.name);
                // print(r.origin + " -> " + hit.point);
                // Debug.DrawLine(r.origin, hit.point, Color.red, 20f);
                if (hit.transform.GetComponent<BubbleBehaviour>() == null) return;

                hit.transform.GetComponent<BubbleBehaviour>().HitBubble(hit.transform.InverseTransformPoint(hit.point));
            }
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

        if (amount > 0)
        {
            IncreaseSize(amount);
        }
        else
        {
            BubleTakeDamage(amount);  
        }
    }

    protected void BubleTakeDamage(float amount)
    {
        _bubble.BubbleSize += amount;
        transform.localScale += new Vector3(1, 1, 1) * (amount / 10);

        if (_takeDmgAnimTimer <= 0)
        {
            _takeDmgAnimTimer = _takeDmgAnimTimerMax;
            StartCoroutine(DamageVisual());   
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

        Vector3 dir = new Vector3(0, 0, -Mathf.Clamp(CurrentHealth, 5, _cameraDistanceMax));
        Quaternion rotation = Quaternion.Euler(mousey, mousex, 0);
        _camera.transform.position = transform.position + rotation * dir;
        _camera.transform.LookAt(transform.position);
    }

    private void HandleInput()
    {
        float x = Input.GetAxis("Horizontal") * _movementSpeed * Time.deltaTime;
        float y = Input.GetAxis("Vertical") * _movementSpeed * Time.deltaTime;


        Vector3 movement = _camera.transform.right * x + _camera.transform.forward * y;
        movement.y = 0f;

        _controller.Move(movement);
        if (movement.magnitude != 0f)
        {
            _bubble.IsMoving = true;
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime);


            Quaternion CamRotation = _camera.transform.rotation;
            CamRotation.x = 0f;
            CamRotation.z = 0f;

            transform.rotation = Quaternion.Lerp(transform.rotation, CamRotation, 0.1f);

        } else
        {
            _bubble.IsMoving = false;
        }
    }
}
