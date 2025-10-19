using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PushableBlock : MonoBehaviour
{
    [Header("Feel")]
    [SerializeField] public float resistance = 1.0f; // higher the  harder to push
    [SerializeField] public float maxSpeed = 3.0f;    // clamp while pushed
    [SerializeField] public bool lockAxisWhenPushed = true;

    private Rigidbody rb;
    private Vector3 pushAxis = Vector3.zero;
    private bool beingPushed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints |= RigidbodyConstraints.FreezeRotation; // keep it upright
    }

    // Called by the player when pushing begins
    public void BeginPush(Vector3 axis)
    {
        pushAxis = axis.normalized;
        beingPushed = true;
    }

    // Called by the player when pushing stops
    public void EndPush()
    {
        beingPushed = false;
        pushAxis = Vector3.zero;
    }

    // Called every FixedUpdate while pushing to �drive� the block
    public void Drive(float desiredSpeed, float pushForce)
    {
        if (!beingPushed || pushAxis == Vector3.zero) return;

        // Current parallel speed
        float curr = Vector3.Dot(rb.linearVelocity, pushAxis);

        // Acceleration scaled by resistance and mass
        float accel = pushForce / (rb.mass * Mathf.Max(0.001f, resistance));
        float next = Mathf.MoveTowards(curr, desiredSpeed, accel * Time.fixedDeltaTime);
        next = Mathf.Clamp(next, -maxSpeed, maxSpeed);

        // Compose new velocity: keep only the axis component (optionally zero sides)
        Vector3 newVel = pushAxis * next;

        if (!lockAxisWhenPushed)
        {
            // Keep a bit of the sideways velocity if you prefer looser feel
            Vector3 side = rb.linearVelocity - pushAxis * curr;
            newVel += side * 0.1f; // small bleed
        }

        rb.linearVelocity = newVel;
    }
}
