using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Transform cam;

    [Header("Character")]
    [SerializeField] private Image _sprite;
    [SerializeField] private Light _lightSource;
    [SerializeField] private Transform _leftSpawn;
    [SerializeField] private Transform _rightSpawn;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float groundAcceleration = 20f;
    [SerializeField] private float groundDeceleration = 25f;

    [Header("Air Control")]
    [SerializeField] private float airAcceleration = 3f;  // small value is better
    [SerializeField] private float airMaxSpeed = 6f;      

    [Header("Facing")]
    [SerializeField] private bool rotateToMove = true;
    [SerializeField] private float rotateSpeed = 720f;

    [Header("Jumping")]
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private float jumpHeight = 2f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private LayerMask groundMask = ~0;

    private Rigidbody rb;
    private int jumpsUsed;
    private bool isGrounded;
    private bool jumpQueued;

    
    private readonly Collider[] _groundHits = new Collider[4];

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        if (!cam && Camera.main) cam = Camera.main.transform;
    }

    void Update()
    {
        //  ground check that ignores our own colliders
        if (groundCheck)
        {
            int count = Physics.OverlapSphereNonAlloc(
                groundCheck.position,
                groundRadius,
                _groundHits,
                groundMask,
                QueryTriggerInteraction.Ignore
            );

            isGrounded = false;
            for (int i = 0; i < count; i++)
            {
                var col = _groundHits[i];
                if (!col) continue;
                if (col.transform.IsChildOf(transform)) continue; // ignore self
                isGrounded = true;
                break;
            }
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
        //wasd movement
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 input = new Vector2(h, v);
        if (input.sqrMagnitude > 1f) input.Normalize(); // no diagonal boost

        if (input.x <= 0f)
        {
            _sprite.rectTransform.localScale = new Vector3(1, 1, 1);
            _lightSource.transform.position = _leftSpawn.position;
        }
        else
        {
            _sprite.rectTransform.localScale = new Vector3(-1, 1, 1);
            _lightSource.transform.position = _rightSpawn.position;
        }
        
        Vector3 fwd = Vector3.forward, right = Vector3.right;
        if (cam)
        {
            fwd = cam.forward; fwd.y = 0f; fwd.Normalize();
            right = cam.right; right.y = 0f; right.Normalize();
        }

        Vector3 desiredDir = (right * input.x + fwd * input.y);
        if (desiredDir.sqrMagnitude > 1f) desiredDir.Normalize();

        Vector3 vel = rb.linearVelocity;
        Vector3 planar = new Vector3(vel.x, 0f, vel.z);

        if (isGrounded)
        {
            // Grounded acceleratedecelerate 
            Vector3 desiredPlanar = desiredDir * moveSpeed;
            Vector3 delta = desiredPlanar - planar;
            float accel = desiredDir.sqrMagnitude > 0.0001f ? groundAcceleration : groundDeceleration;
            Vector3 accelVec = Vector3.ClampMagnitude(delta, accel);
            rb.AddForce(accelVec, ForceMode.Acceleration);
        }
        else
        {
            //small acceleration in air 
            if (desiredDir.sqrMagnitude > 0f)
            {
                rb.AddForce(desiredDir * airAcceleration, ForceMode.Acceleration);
            }

            // Keep  speed under control in air
            Vector3 newPlanar = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            float spd = newPlanar.magnitude;
            if (spd > airMaxSpeed)
            {
                newPlanar = newPlanar.normalized * airMaxSpeed;
                rb.linearVelocity = new Vector3(newPlanar.x, rb.linearVelocity.y, newPlanar.z);
            }
        }

        // Jump
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

        // facing
        if (rotateToMove)
        {
            Vector3 face = desiredDir.sqrMagnitude > 0.001f ? desiredDir : planar.normalized;
            if (face.sqrMagnitude > 0.001f)
            {
                Quaternion target = Quaternion.LookRotation(face, Vector3.up);
                rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, target, rotateSpeed * Time.fixedDeltaTime));
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (groundCheck)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }
#endif
}
