using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.SceneView;
using UnityEngine.InputSystem.XR;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.GraphicsBuffer;

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

    void Start()
    {
        _camera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        _controller = GetComponent<CharacterController>();
        //_camera.transform.SetParent(transform, false);
    }

    // Update is called once per frame
    void Update()
    {

        //if(isGameActive) return;




        HandleInput();
    }

    void LateUpdate()
    {
        MouseCameraRotation();
    }


    private void MouseCameraRotation()
    {
        mousex += Input.GetAxis("Mouse X") * _mouseSensitivity * Time.fixedUnscaledDeltaTime;
        mousey += Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.fixedUnscaledDeltaTime;

        mousey = Mathf.Clamp(mousey, 5, 50);

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
