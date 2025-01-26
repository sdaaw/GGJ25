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

    private BubbleCharacterController _player;
    private Vector3 _targetSphereStartScale;

    [SerializeField]
    private RenderTexture _renderTexture;

    private void Awake()
    {
        ShootMode = false;
        if(_targetSpherePrefab)
        {
            _targetSphere = GameObject.Instantiate(_targetSpherePrefab, transform.position, Quaternion.identity);
        }
        _player = GetComponent<BubbleCharacterController>();
        _targetSphereStartScale = _targetSphere.transform.localScale;
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

        _renderTexture = Camera.main.targetTexture;
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
                    lineRenderer.startWidth = lineWidth + (_player.CurrentHealth / 150);
                    lineRenderer.endWidth = lineWidth + (_player.CurrentHealth / 150);
                    _targetSphere.transform.localScale = _targetSphereStartScale * _player.CurrentHealth / 10 * 1.25f;
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

        var mousePos = Input.mousePosition;

        if (_renderTexture != null)
        {
            // Divide by screen size and multiply by render texture size
            mousePos = mousePos / new Vector2(UnityEngine.Screen.width, UnityEngine.Screen.height) * new Vector2(_renderTexture.width, _renderTexture.height);
        }

        var ray = Camera.main.ScreenPointToRay(mousePos);

        var hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~IgnoreLayer))
        {
            _mousePos = hit.point;
        }
        // + (_player.CurrentHealth / 10 * 2)
        // clamp mouseposition to max radius
        var difference = _mousePos - transform.position;
        var direction = difference.normalized;
        var distance = Mathf.Min(maxRadius + (_player.CurrentHealth / 10 * 2), difference.magnitude);
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


        if (Input.GetMouseButtonDown(0) && ShootMode)
        {
            // Debug.Log(endPosition);
            // TODO: hold to fire bigger ammo at cost of more mass
            var player = GetComponent<BubbleCharacterController>();
            player.CurrentHealth -= projectileCost + (player.CurrentHealth / 25);

            var projectile = GameObject.Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
            var p = projectile.GetComponent<Projectile>();
            if(p)
            {
                p.startPos = transform.position;
                p.finalPosition = endPosition;
                p.curveHeight = curveHeight;
                p.velocity = 15;
                p.dmg = projectileCost + (player.CurrentHealth / 10) * 2;
                p.owner = transform;
                p.transform.localScale *= (player.CurrentHealth / 10) * 1.25f;
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