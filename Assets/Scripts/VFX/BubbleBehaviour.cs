using UnityEngine;

public class BubbleBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Gradient _gradient;


    [SerializeField]
    private float _displacementSpeedMin, _displacementSpeedMax, _displacementPower;

    private Renderer _renderer;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material.SetFloat("_DisplacementSpeed", Random.Range(_displacementSpeedMin, _displacementSpeedMax));
        _renderer.material.SetFloat("_DisplacementPower", _displacementPower);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
