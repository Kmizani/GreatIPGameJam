using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private bool captureStartAsDefault = true;

    private Vector3 startPos;
    private Quaternion startRot;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (captureStartAsDefault && !respawnPoint)
        {
            startPos = transform.position;
            startRot = transform.rotation;
        }
    }

    public void Respawn()
    {
        Vector3 pos = respawnPoint ? respawnPoint.position : startPos;
        Quaternion rot = respawnPoint ? respawnPoint.rotation : startRot;

        if (rb)
        {
            rb.linearVelocity = Vector3.zero; rb.angularVelocity = Vector3.zero;
            rb.position = pos; rb.rotation = rot;
        }
        else transform.SetPositionAndRotation(pos, rot);
    }

    public void SetRespawnPoint(Transform t) => respawnPoint = t;
}
