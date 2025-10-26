using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UIElements;
using System.Collections;

public class EndOfPuzzle : MonoBehaviour
{
    [SerializeField] private GameObject _gem;
    [SerializeField] private float _rotX = -90;

    private void Start()
    {
        _gem.SetActive(false);
    }

    private void VentFall()
    {
        Debug.Log("Vent Fall");
        StartCoroutine(RotateVent());
    }

    private IEnumerator RotateVent()
    {
        Quaternion startRot = transform.localRotation;
        Quaternion endRot = Quaternion.Euler(_rotX, 0, 0);
        float elapsed = 0f;

        while (elapsed < 1)
        {
            transform.localRotation = Quaternion.Lerp(startRot, endRot, elapsed / 1);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = endRot; // snap to final rotation
    }

    private void OnEnable()
    {
        //WordPuzzle.endOfLevel += VentFall;  
    }

    private void OnDisable()
    {
        //WordPuzzle.endOfLevel -= VentFall;
    }
}
