using System.Collections.Generic;
using UnityEngine;

public class DrawLineRenderer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int vertexCount = 32; // Number of vertices in the curve
    public float lineWidth = 0.2f; // Width of the line
    public float curveHeight = 10;

    [SerializeField]
    private Vector3 _mousePos;

    private GameObject _targetSphere;

    [SerializeField]
    private GameObject _targetSpherePrefab;


    private void Awake()
    {
        if(_targetSpherePrefab)
        {
            _targetSphere = GameObject.Instantiate(_targetSpherePrefab, transform.position, Quaternion.identity);
            // _targetSphere.SetActive(false);
        }
    }

    private void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }

    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        var hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            _mousePos = hit.point;
        }

        var pointList = new List<Vector3>();

        for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount)
        {
            var tangent1 = Vector3.Lerp(transform.position, (transform.position + _mousePos) / 2 + Vector3.up * curveHeight, ratio);
            var tangent2 = Vector3.Lerp((transform.position + _mousePos) / 2 + Vector3.up * curveHeight, _mousePos, ratio);
            var curve = Vector3.Lerp(tangent1, tangent2, ratio);
            pointList.Add(curve);
        }

        lineRenderer.positionCount = pointList.Count;
        lineRenderer.SetPositions(pointList.ToArray());

        _targetSphere.transform.position = _mousePos;


        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(_mousePos);
        }
    }
}