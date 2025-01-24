using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BubbleCharacterController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    private float _movementSpeed;

    [SerializeField]
    private float _mouseSensitivity, _cameraFov;


    private Vector3 _cameraOffset;

    [SerializeField]
    private float _cameraDistance;

    private Camera _camera;

    private float _xrotation;


    void Start()
    {
        _camera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        //_camera.transform.SetParent(transform, false);
    }

    // Update is called once per frame
    void Update()
    {

        //if(isGameActive) return;

        /*
        _camera.transform.position = new Vector3(
            transform.position.x + _cameraOffset.x,
            transform.position.y + _cameraOffset.y,
            transform.position.z + _cameraOffset.z);*/

        HandleInput();
    }

    void LateUpdate()
    {
        MouseCameraRotation();
    }

    private void MouseCameraRotation()
    {
        _cameraOffset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * _mouseSensitivity, Vector3.up) * _cameraOffset;
        _cameraOffset = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * _mouseSensitivity, Vector3.right) * _cameraOffset;

        //_cameraOffset = new Vector3(_cameraOffset.x, _cameraOffset.y, _cameraOffset.z);

        _camera.transform.localPosition = _cameraOffset;
        _camera.transform.LookAt(transform.position);
        /*float x = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.fixedUnscaledDeltaTime;
        float y = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.fixedUnscaledDeltaTime;
        y = Mathf.Clamp(y, -90f, 90f); 
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -_cameraOffset.z) + transform.position;

        _camera.transform.LookAt(transform);
        
        _camera.transform.rotation = rotation;
        _camera.transform.position = position;*/
        /*
        _xrotation -= y;
        _xrotation = Mathf.Clamp(_xrotation, -90f, 90f);
        _camera.transform.localRotation = Quaternion.Euler(_xrotation, 0f, 0f);*/
    }

    private void HandleInput()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        print(x + ", " + y);
    }
}
