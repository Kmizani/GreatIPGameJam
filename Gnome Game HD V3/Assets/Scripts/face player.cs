using UnityEngine;

public class NPCFacePlayerYawOnly : MonoBehaviour
{
    [SerializeField] private Transform target;         
    [SerializeField] private float yawOffsetDeg = 0f;   
    [SerializeField] private bool pinPosition = true;   

    private Vector3 anchor;

    void Awake()
    {
        anchor = transform.position;
    }

    void LateUpdate()
    {
        if (!target) return;

        if (pinPosition) transform.position = anchor;

        Vector3 dir = target.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 1e-6f) return;

        float yaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, yaw + yawOffsetDeg, 0f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, transform.forward * 1.5f);
    }
}
