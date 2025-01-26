using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Projectile : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 finalPosition;
    public float curveHeight;
    public float velocity = 15;
    public float dmg = 1;
    public Transform owner;

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

    // projectile needs rigidbody for this to work...
    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log(other.gameObject.name);
        //its not ourself (cant shoot itself)
        if (other.transform != owner)
        {
            //its not a bullet
            if (other.gameObject.layer != 8)
            {
                if (other.gameObject.GetComponent<Entity>())
                    other.gameObject.GetComponent<Entity>().CurrentHealth -= dmg;

                Destroy(gameObject);
            }
        }
    }
}
