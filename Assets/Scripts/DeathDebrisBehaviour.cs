using System.Collections;
using UnityEngine;

public class DeathDebrisBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Vector3[] _curvePoints = new Vector3[4];

    public GameObject player;

    public Vector3 startPos;

    private Renderer _r;
    
    void Start()
    {
        _r = GetComponent<Renderer>();
        _r.material.SetColor("_BubbleColorTint", new Color(Random.Range(0.5f, 1f), Random.Range(0.1f, 0.2f), Random.Range(0.5f, 1f)));
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 3f)
        {
            Destroy(gameObject);
        }

        if (transform.position == _curvePoints[3])
        {
            Destroy(gameObject);
        }
    }

    public void GeneratePoints()
    {
        player = GameManager.instance.player;
        _curvePoints[0] = startPos;

        _curvePoints[1] = new Vector3(
            startPos.x + Random.Range(-10, 10),
            startPos.y + Random.Range(-10, 10),
            startPos.z + Random.Range(-10, 10));

        _curvePoints[2] = new Vector3(
            startPos.x + Random.Range(-10, 10),
            startPos.y + Random.Range(-10, 10),
            startPos.z + Random.Range(-10, 10));

        _curvePoints[3] = player.transform.position;

        StartCoroutine(InterpolateBezier(100, 0.01f));
    }

    IEnumerator InterpolateBezier(int precision, float speed)
    {
        for (int i = 0; i <= precision; i++)
        {
            float t = i / (float)precision;
            transform.position = GetCubicBezierPoint3D(
                _curvePoints[0],
                _curvePoints[1],
                _curvePoints[2],
                _curvePoints[3], t);
            yield return new WaitForSeconds(speed);
        }
    }

    public static Vector2 CubicBezierEval(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float u = 1 - t;
        return u * u * u * p0 + 3 * u * u * t * p1 + 3 * u * t * t * p2 + t * t * t * p3;
    }

    public Vector3 GetCubicBezierPoint3D(Vector3 P0, Vector3 P1, Vector3 P2, Vector3 P3, float t)
    {
        t = Mathf.Clamp01(t);
        Vector3 point = Mathf.Pow(1 - t, 3) * P0 + 
            3 * Mathf.Pow(1 - t, 2) * t * P1 + 
            3 * (1 - t) * Mathf.Pow(t, 2) * P2 + 
            Mathf.Pow(t, 3) * P3; 

        return point;
    }
}
