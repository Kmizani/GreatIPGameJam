using UnityEngine;

public class AutoFlip : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "player")
        {
            other.GetComponent<CharacterController>().SetFlip(true);
        }
    }
}
