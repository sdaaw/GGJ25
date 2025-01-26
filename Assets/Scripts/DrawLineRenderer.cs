using System.Collections.Generic;
using UnityEngine;

public class DrawLineRenderer : MonoBehaviour
{
    public LayerMask IgnoreLayer;
    public LineRenderer lineRenderer;
    public int vertexCount = 32; // Number of vertices in the curve
    public float lineWidth = 0.2f; // Width of the line
    public float curveHeight = 15;
    public float maxRadius = 20;
    public bool ShootMode = false;
    public float projectileCost = 1;

    [SerializeField]
    private Vector3 _mousePos;

    private GameObject _targetSphere;

    [SerializeField]
    private GameObject _targetSpherePrefab;

    [SerializeField]
    private GameObject _projectilePrefab;

    private List<Vector3> pointList = new List<Vector3>();

    private void Awake()
    {
        ShootMode = false;
        if(_targetSpherePrefab)
        {
            _targetSphere = GameObject.Instantiate(_targetSpherePrefab, transform.position, Quaternion.identity);
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

        lineRenderer.enabled = false;
        if (_targetSphere != null)
        {
            _targetSphere.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ShootMode = !ShootMode;
            if (ShootMode)
            {
                Cursor.lockState = CursorLockMode.Confined;
                lineRenderer.enabled = true;
                if (_targetSphere != null)
                {
                    _targetSphere.SetActive(true);
                    // TODO: disable movement?
                }
            } 
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                lineRenderer.enabled = false;
                if (_targetSphere != null)
                {
                    _targetSphere.SetActive(false);
                }
            }
        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        var hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, 100, ~IgnoreLayer))
        {
            _mousePos = hit.point;
        }

        // clamp mouseposition to max radius
        var difference = _mousePos - transform.position;
        var direction = difference.normalized;
        var distance = Mathf.Min(maxRadius, difference.magnitude);
        var endPosition = transform.position + direction * distance;

        pointList.Clear();

        for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount)
        {
            var tangent1 = Vector3.Lerp(transform.position, (transform.position + endPosition) / 2 + Vector3.up * curveHeight, ratio);
            var tangent2 = Vector3.Lerp((transform.position + endPosition) / 2 + Vector3.up * curveHeight, endPosition, ratio);
            var curve = Vector3.Lerp(tangent1, tangent2, ratio);
            pointList.Add(curve);
        }

        lineRenderer.positionCount = pointList.Count;
        lineRenderer.SetPositions(pointList.ToArray());

        _targetSphere.transform.position = endPosition;


        if (Input.GetMouseButtonDown(0))
        {
            // Debug.Log(endPosition);
            // TODO: hold to fire bigger ammo at cost of more mass

            GetComponent<BubbleCharacterController>().CurrentHealth -= projectileCost;

            var projectile = GameObject.Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
            var p = projectile.GetComponent<Projectile>();
            if(p)
            {
                p.startPos = transform.position;
                p.finalPosition = endPosition;
                p.curveHeight = curveHeight;
                p.velocity = 15;
                p.dmg = projectileCost;
                p.owner = transform;
            }

            ShootMode = false;

            Cursor.lockState = CursorLockMode.Locked;
            lineRenderer.enabled = false;
            if (_targetSphere != null)
            {
                _targetSphere.SetActive(false);
            }
        }
    }
}