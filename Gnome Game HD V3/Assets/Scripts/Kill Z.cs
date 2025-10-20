using UnityEngine;

[RequireComponent(typeof(Collider))]
public class KillZVolume : MonoBehaviour
{
    [SerializeField] private string playerTag = "player";
    [SerializeField] private Transform playerRespawnPoint;
    [SerializeField] private LayerMask reactLayers = ~0;
    [SerializeField] private bool logHits = false;

    void Reset()
    {
        var c = GetComponent<Collider>();
        c.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & reactLayers) == 0) return;

        var root = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;

        if (root.CompareTag(playerTag))
        {
            RespawnPlayer(root);
            return;
        }

        var responder = root.GetComponentInParent<KillZResponder>();
        if (responder) responder.HandleKillZ(this);

        if (logHits) Debug.Log($"KillZ hit: {root.name}", root);
    }

    void RespawnPlayer(GameObject player)
    {
        var pr = player.GetComponentInParent<PlayerRespawn>();
        if (pr) { pr.Respawn(); return; }

        if (playerRespawnPoint)
        {
            if (player.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.linearVelocity = Vector3.zero; rb.angularVelocity = Vector3.zero;
                rb.position = playerRespawnPoint.position; rb.rotation = playerRespawnPoint.rotation;
            }
            else player.transform.SetPositionAndRotation(playerRespawnPoint.position, playerRespawnPoint.rotation);
        }
    }

    public Transform GetRespawnPoint() => playerRespawnPoint;
}
