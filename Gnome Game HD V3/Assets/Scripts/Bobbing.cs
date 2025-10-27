using UnityEngine;

public class Bobbing : MonoBehaviour
{
    [SerializeField] private float bobSpeed = 1.5f; // Speed of bobbing
    [SerializeField] private float bobAmount = 0.05f; // Height of bob
    private Vector3 _startPos;

    void Start()
    {
        _startPos = transform.localPosition;
    }

    void Update()
    {
        float newY = _startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobAmount;
        transform.localPosition = new Vector3(_startPos.x, newY, _startPos.z);
    }
}
