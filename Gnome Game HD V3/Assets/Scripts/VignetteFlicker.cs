using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class VignetteFlicker : MonoBehaviour
{
    [SerializeField] private Volume volume;
    [SerializeField] private float flickerSpeed = 5f;
    [SerializeField] private float minIntensity = 0.2f;
    [SerializeField] private float maxIntensity = 0.4f;
    [SerializeField] private Color _colour;
    [SerializeField] private float _duration = 5f;

    private Vignette _vignette;
    private float _timer;

    private void OnEnable()
    {
        GemCollection._gemCollected += Update;
    }
    private void OnDisable()
    {
        GemCollection._gemCollected -= Update;
    }


    void Start()
    {
        if (volume.profile.TryGet(out Vignette vignette))
            _vignette = vignette;
        _vignette.active = true;
        _vignette.intensity.overrideState = true;
        _vignette.color.overrideState = true;


    }

    void Update()
    {
        if (_vignette == null) return;
        _vignette.color.value = _colour;

       if(_timer < _duration)
        {
            _timer += Time.deltaTime * flickerSpeed;
            float t = (Mathf.Sin(_timer) + 1f) / 2f;
            _vignette.intensity.value = Mathf.Lerp(minIntensity, maxIntensity, t);
        }
        _vignette.intensity.value = 0f;
    }
}
