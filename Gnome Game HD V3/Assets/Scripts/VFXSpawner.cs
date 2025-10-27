using UnityEngine;

public class VFXSpawner : MonoBehaviour
{
    [SerializeField] private GameObject vfxPrefab;
    [SerializeField] private float destroyAfterSeconds = 2f;

    public void SpawnVFX()
    {
        if (vfxPrefab == null)
        {
            Debug.LogWarning("VFX prefab not assigned!");
            return;
        }

        GameObject vfx = Instantiate(vfxPrefab);
        Destroy(vfx, destroyAfterSeconds);
    }
}
