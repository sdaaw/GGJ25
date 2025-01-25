using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    private float _skyboxTime;

    [SerializeField]
    private bool _isFrozenInTime;

    [SerializeField]
    private float _skyBoxSpeed;

    [SerializeField]
    private Material _skyBoxMaterial;

    public float SkyboxSpeed
    {
        get
        {
            return _isFrozenInTime ? 0 : _skyBoxSpeed;
        }
        set
        {
            _skyBoxSpeed = value;
            _skyBoxMaterial.SetFloat("_SkyTime", _skyBoxSpeed);
        }
    }

    void Start()
    {
        if (_skyBoxMaterial == null)
        {
            Debug.LogWarning("SkyboxMaterial is not set");
            return;
        }

        SkyboxSpeed = 1.5f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }
}
