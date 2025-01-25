using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform _transform;
    private Camera _cam;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _cam = FindFirstObjectByType<Camera>();

    }

    private void Update()
    {
        _transform.LookAt(_cam.transform.position);
    }
}