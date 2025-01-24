using UnityEngine;

public class BubbleBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Gradient _gradient;


    [SerializeField]
    private float _displacementSpeedMin, _displacementSpeedMax, _displacementPower;

    private Renderer _renderer;

    [HideInInspector]
    public float BubbleSize;

    [SerializeField]
    private float _bubbleStartSize;

    private float _bubbleSpeed;


    public float DisplacementPower 
    { 
        get 
        { 
            return _displacementPower; 
        } 
        set 
        {
            _displacementPower = value;
            _renderer.material.SetFloat("_DisplacementPower", _displacementPower);
        } 
    }

    public float DisplacementSpeed
    {
        get 
        {
            return _bubbleSpeed;
        }
        set
        {
            _bubbleSpeed = value;
            _renderer.material.SetFloat("_DisplacementSpeed", _bubbleSpeed);
        }
    }

    void Start()
    {
        BubbleSize = _bubbleStartSize;
        _renderer = GetComponent<Renderer>();
        _bubbleSpeed = Random.Range(_displacementSpeedMin, _displacementSpeedMax);
        _renderer.material.SetFloat("_DisplacementSpeed", _bubbleSpeed);
        _renderer.material.SetFloat("_DisplacementPower", _displacementPower);
        transform.localScale = new Vector3(_bubbleStartSize, _bubbleStartSize, _bubbleStartSize);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseSize()
    {

    }

}
