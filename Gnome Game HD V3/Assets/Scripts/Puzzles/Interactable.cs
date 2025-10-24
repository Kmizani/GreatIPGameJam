using System;
using Unity.VisualScripting;
using UnityEngine;

 public class Interactable : MonoBehaviour
{
    [SerializeField] GameObject UI;
    private bool _playerInRange;
    public event Action triggerEvent;

    private void Start()
    {
        if (UI != null) UI.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "player")
        {
            _playerInRange = true;
            Debug.Log("player entered");
            if(UI != null) UI.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "player")
        {
            _playerInRange = false;
            Debug.Log("player exited");
            if (UI != null) UI.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // only allow interaction when inside the trigger
        if (_playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }
     public void Interact()
    {
        triggerEvent?.Invoke();
        Debug.Log("interacted");
    }
}
