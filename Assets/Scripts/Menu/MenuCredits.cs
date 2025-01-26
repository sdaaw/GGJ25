using System.Collections;
using TMPro;
using UnityEngine;

public class MenuCredits : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    private string[] names;

    public GameObject[] nameTexts;

    void Start()
    {
    }

    private void OnEnable()
    {
        StartCoroutine(DoVisual());
    }

    // Update is called once per frame

    IEnumerator DoVisual()
    {
        yield return new WaitForSeconds(0.2f);
        foreach (GameObject text in nameTexts)
        {
            text.GetComponent<MenuCreditsNameElement>().ToggleSelect();
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(DoVisual());
    }
}
