using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputActionListener : MonoBehaviour
{
    [SerializeField] private InputActionReference _actionReference;
    [SerializeField] private Button _activateButton;

    public UnityEvent OnPressed;

    private void OnEnable()
    {
        // adding listener to C# event, we use += instead of AddListener function
        _actionReference.action.performed += Performed;
    }

    private void OnDisable()
    {
        _actionReference.action.performed -= Performed;
    }

    private void Performed(InputAction.CallbackContext context)
    {
        OnPressed.Invoke();
    }

    public void ForceActivate()
    {
        OnPressed.Invoke();
    }
}
