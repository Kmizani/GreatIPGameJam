using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class IsoCharacterController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Transform cam;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float deceleration = 25f;
    [SerializeField, Range(0f, 1f)] private float airControl = 0.6f;
    [SerializeField] private bool rotateToMove = true;
    [SerializeField] private float rotateSpeed = 720f;

    [Header("Jumping")]
    [SerializeField] private int maxJumps = 2;   // set 2 for double-jump
    [SerializeField] private float jumpHeight = 2f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private LayerMask groundMask = ~0;

    private Rigidbody rb;
    private int jumpsUsed;
    private bool isGrounded;
    private bool jumpQueued;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        if (cam == null && Camera.main != null) cam = Camera.main.transform;
    }

    void Update()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(
                groundCheck.position,
                groundRadius,
                groundMask,
                QueryTriggerInteraction.Ignore
            );
        }
        else
        {
            isGrounded = Mathf.Abs(rb.linearVelocity.y) < 0.05f;
        }

        if (isGrounded) jumpsUsed = 0;

        if (Input.GetKeyDown(KeyCode.Space))
            jumpQueued = true;
    }

    void FixedUpdate()
    {
        // Legacy axes (exist by default): Horizontal (A/D), Vertical (W/S)
        Vector2 raw = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 input = raw.sqrMagnitude > 1f ? raw.normalized : raw; // remove diagonal boost

        Vector3 fwd = Vector3.forward, right = Vector3.right;
        if (cam != null)
        {
            fwd = cam.forward; fwd.y = 0f; fwd.Normalize();
            right = cam.right; right.y = 0f; right.Normalize();
        }

        Vector3 desiredDir = right * input.x + fwd * input.y;
        if (desiredDir.sqrMagnitude > 1f) desiredDir.Normalize();

        Vector3 desiredPlanarVel = desiredDir * moveSpeed;

        Vector3 vel = rb.linearVelocity;
        Vector3 planarVel = new Vector3(vel.x, 0f, vel.z);

        Vector3 delta = desiredPlanarVel - planarVel;
        float curAccel = desiredDir.sqrMagnitude > 0.0001f ? acceleration : deceleration;
        float control = isGrounded ? 1f : airControl;

        Vector3 accelVec = Vector3.ClampMagnitude(delta, curAccel * control);
        rb.AddForce(accelVec, ForceMode.Acceleration);

        if (jumpQueued)
        {
            if (isGrounded || jumpsUsed < maxJumps)
            {
                float upSpeed = Mathf.Sqrt(2f * jumpHeight * -Physics.gravity.y);
                vel = rb.linearVelocity; vel.y = 0f; rb.linearVelocity = vel;
                rb.AddForce(Vector3.up * upSpeed, ForceMode.VelocityChange);
                jumpsUsed++;
            }
            jumpQueued = false;
        }

        if (rotateToMove)
        {
            Vector3 faceDir = desiredDir.sqrMagnitude > 0.001f ? desiredDir : planarVel.normalized;
            if (faceDir.sqrMagnitude > 0.001f)
            {
                Quaternion target = Quaternion.LookRotation(faceDir, Vector3.up);
                rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, target, rotateSpeed * Time.fixedDeltaTime));
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }
#endif
}
