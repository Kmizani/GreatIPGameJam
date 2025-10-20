using UnityEngine;

public class PlayerPusherSimple : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float detectRadius = 0.45f;   // chest hit radius
    [SerializeField] private float detectDistance = 0.75f; // how far ahead to look
    [SerializeField] private LayerMask pushableMask = ~0;  // set to Pushable layer
    [SerializeField] private Transform castOrigin;         // defaults to player position

    [Header("Push Feel")]
    [SerializeField] private float pushSpeed = 2.5f;       // target speed while pushing
    [SerializeField] private float pushForce = 30f;        // acceleration toward target speed
    [SerializeField] private float releaseDistance = 1.1f; // let go if  drift away

    private PushableBlock current;
    private Vector3 pushAxis; // world-space X/Z axis that its driving along
    private Rigidbody playerRb;

    void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        if (!castOrigin) castOrigin = transform;
    }

    void FixedUpdate()
    {
        // Read simple WASD 
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        bool jumpPressed = Input.GetKey(KeyCode.Space);
        Vector3 wishMove = new Vector3(h, 0f, v);

        // Map wishMove to world based on player facing so “forward” means the way the player faces
        Vector3 fwd = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized;
        Vector3 moveWorld = (right * h + fwd * v);
        if (moveWorld.sqrMagnitude > 1f) moveWorld.Normalize();

        if (current == null)
        {
            TryBeginPush(moveWorld);
        }
        else
        {
            // Release conditions
            if (jumpPressed || moveWorld.sqrMagnitude < 0.0001f)
            {
                StopPushing();
                return;
            }

            // Still close and still in front?
            if (!StillInFrontOfSameBlock())
            {
                StopPushing();
                return;
            }

            // Drive along axis using player intention projected onto that axis
            float inputAlongAxis = Vector3.Dot(moveWorld, pushAxis); // [-1, 1]
            float desiredSpeed = inputAlongAxis * pushSpeed;

            current.Drive(desiredSpeed, pushForce);
        }
    }

    void TryBeginPush(Vector3 moveWorld)
    {
        if (moveWorld.sqrMagnitude < 0.0001f) return; // not trying to move

        Vector3 origin = castOrigin.position + Vector3.up * 0.1f;
        Vector3 dir = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        if (dir.sqrMagnitude < 1e-6f) return;

        // SphereCast to find a pushable right in front
        if (Physics.SphereCast(origin, detectRadius, dir, out RaycastHit hit, detectDistance, pushableMask, QueryTriggerInteraction.Ignore))
        {
            var block = hit.collider.GetComponentInParent<PushableBlock>();
            if (!block) return;

            // Only start pushing if we're actually pushing into the face 
            float facingDot = Vector3.Dot(dir, -hit.normal);
            if (facingDot < 0.5f) return; // not square enough

            // Axis is opposite of face normal, flattened to ground and snapped to X/Z
            Vector3 axis = Vector3.ProjectOnPlane(-hit.normal, Vector3.up);
            if (axis.sqrMagnitude < 1e-6f) return;
            axis = SnapToCardinalXZ(axis.normalized); // keep it perfectly straight

            current = block;
            pushAxis = axis;
            current.BeginPush(pushAxis);
        }
    }

    bool StillInFrontOfSameBlock()
    {
        if (!current) return false;
        Vector3 origin = castOrigin.position + Vector3.up * 0.1f;
        Vector3 dir = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;

        if (Physics.SphereCast(origin, detectRadius, dir, out RaycastHit hit, detectDistance * 1.1f, pushableMask, QueryTriggerInteraction.Ignore))
        {
            return hit.collider && hit.collider.GetComponentInParent<PushableBlock>() == current;
        }

        // Also allow a small distance buffer even without direct cast hit
        float dist = Vector3.Distance(transform.position, current.transform.position);
        return dist < releaseDistance;
    }

    void StopPushing()
    {
        if (current)
        {
            current.EndPush();
            current = null;
            pushAxis = Vector3.zero;
        }
    }

    static Vector3 SnapToCardinalXZ(Vector3 v)
    {
        v.y = 0f;
        float ax = Mathf.Abs(v.x);
        float az = Mathf.Abs(v.z);
        if (ax >= az) return new Vector3(Mathf.Sign(v.x), 0f, 0f);
        else return new Vector3(0f, 0f, Mathf.Sign(v.z));
    }
}
