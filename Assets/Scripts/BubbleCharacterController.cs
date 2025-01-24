using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.SceneView;
using UnityEngine.InputSystem.XR;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.GraphicsBuffer;
using System.Collections;

public class BubbleCharacterController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    private float _movementSpeed;

    [SerializeField]
    private float _mouseSensitivity, _cameraFov;


    [SerializeField]
    private float _cameraDistance;

    private Camera _camera;

    private float mousey, mousex;

    private CharacterController _controller;

    private BubbleBehaviour _bubble;

    [SerializeField]
    private float _ycameraClampMin, _ycameraClampMax;
    void Start()
    {
        _camera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        _controller = GetComponent<CharacterController>();
        _bubble = GetComponent<BubbleBehaviour>();
        //_camera.transform.SetParent(transform, false);
    }

    // Update is called once per frame
    void Update()
    {

        //if(isGameActive) return;
        if(Input.GetKeyDown(KeyCode.Q))
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
            TakeDamage(0.1f);
        }

        HandleInput();
    }

    public void TakeDamage(float amount)
    {
        _bubble.BubbleSize -= amount;
        StartCoroutine(DamageVisual());
    }

    IEnumerator DamageVisual()
    {
        float visualIntensity = 4f;
        _bubble.DisplacementPower *= visualIntensity;
        _bubble.DisplacementSpeed *= visualIntensity;
        yield return new WaitForSeconds(0.5f);
        _bubble.DisplacementPower /= visualIntensity;
        _bubble.DisplacementSpeed /= visualIntensity;
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

        Vector3 dir = new Vector3(0, 0, -_cameraDistance);
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
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime);


            Quaternion CamRotation = _camera.transform.rotation;
            CamRotation.x = 0f;
            CamRotation.z = 0f;

            transform.rotation = Quaternion.Lerp(transform.rotation, CamRotation, 0.1f);

        }
    }
}
