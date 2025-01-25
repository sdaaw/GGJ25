using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 finalPosition;
    public float curveHeight;
    public float velocity = 15;

    private void Start()
    {
        transform.position = startPos;
        StartCoroutine(moveProjectile());
    }

    private void FixedUpdate()
    {
        /*for (float ratio = 0; ratio <= 1; ratio += 1.0f / 32)
        {
            // yield return new WaitForSeconds(0.025f);
            var tangent1 = Vector3.Lerp(startPos, (transform.position + finalPosition) / 2 + Vector3.up * curveHeight, ratio);
            var tangent2 = Vector3.Lerp((transform.position + finalPosition) / 2 + Vector3.up * curveHeight, finalPosition, ratio);
            var curve = Vector3.Lerp(tangent1, tangent2, ratio);
            // pointList.Add(curve);
            transform.position = curve;
        }*/
    }

    private IEnumerator moveProjectile()
    {
        for (float ratio = 0; ratio <= 1; ratio += 1.0f / 32)
        {
            yield return new WaitForSeconds(0.02f);
            var tangent1 = Vector3.Lerp(startPos, (transform.position + finalPosition) / 2 + Vector3.up * curveHeight, ratio);
            var tangent2 = Vector3.Lerp((transform.position + finalPosition) / 2 + Vector3.up * curveHeight, finalPosition, ratio);
            var curve = Vector3.Lerp(tangent1, tangent2, ratio);
            transform.position = curve;
        }
    }
}
