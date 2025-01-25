using UnityEngine;

public class BubbleBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Gradient _gradient;


    [SerializeField]
    private float _displacementSpeedMin, _displacementSpeedMax, _displacementPower, _outerGlowWidth;

    private Renderer _renderer;

    [HideInInspector]
    public float BubbleSize;

    [SerializeField]
    private float _bubbleStartSize;

    private float _bubbleSpeed;

    [SerializeField]
    private Color _colorTint;


    public Color damagedColorTint;

    public Color BubbleColorTint
    {
        get
        {
            return _colorTint;
        }
        set
        {
            _colorTint = value;
            _renderer.material.SetColor("_BubbleColorTint", _colorTint);
        }
    }


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

    public float OuterGlowWidth
    {
        get
        {
            return _outerGlowWidth;
        }
        set
        {
            _outerGlowWidth = value;
            _renderer.material.SetFloat("_OuterGlow", _outerGlowWidth);
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

    public void HitBubble(Vector3 pos)
    {
        print(pos);
        if (pos == Vector3.zero) return;
        _renderer.material.SetVector("_HitPoint", pos);
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
}
