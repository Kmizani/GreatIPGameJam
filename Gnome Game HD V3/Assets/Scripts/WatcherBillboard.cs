using UnityEngine;

public class WatcherBillboard : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;            // Player
    [SerializeField] private bool useMainCameraIfNone;  

    [Header("Rotation")]
    [SerializeField] private bool yawOnly = true;         // Keep upright
    [SerializeField] private float turnSpeed = 0f;        // 0  to snap instantly

    private Vector3 anchorPos;                            // Hard pin 
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anchorPos = transform.position;

        // make it kinematic so physics can't push it.
        if (rb) rb.isKinematic = true;
    }

    void LateUpdate()
    {
        // assert exact world position every frame
        if (rb) rb.position = anchorPos; else transform.position = anchorPos;

        // Resolve target
        if (!target && useMainCameraIfNone && Camera.main) target = Camera.main.transform;
        if (!target) return;

        // Compute facing
        Vector3 toTarget = target.position - anchorPos;
        if (yawOnly) toTarget.y = 0f;
        if (toTarget.sqrMagnitude < 1e-8f) return;

        Quaternion desired = Quaternion.LookRotation(toTarget.normalized, Vector3.up);
        Quaternion newRot = (turnSpeed <= 0f)
            ? desired
            : Quaternion.RotateTowards(transform.rotation, desired, turnSpeed * Time.deltaTime);

        if (rb) rb.MoveRotation(newRot); else transform.rotation = newRot;
    }

   
    public void ResetAnchorHere() => anchorPos = transform.position;

    public void SetTarget(Transform t) => target = t;
}
