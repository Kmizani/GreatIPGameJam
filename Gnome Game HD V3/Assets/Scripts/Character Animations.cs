using UnityEngine;

public class CharacterAnimations: MonoBehaviour
{
    [SerializeField] private float wobbleAmount = 10f;  // angle or height
    [SerializeField] private float wobbleSpeed = 5f;    // how fast it moves
    [SerializeField] private bool bobInstead = false;   // toggle bobbing vs rotation
    [SerializeField] private Transform character;       // reference to player

    private Vector3 startPos;
    private Vector3 startRot;
    private float timer;

    void Start()
    {
        startPos = transform.localPosition;
        startRot = transform.localEulerAngles;
    }

    void Update()
    {
        if (character == null)
            return;

        // get player movement input (WASD or arrow keys)
        float move = Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Mathf.Abs(Input.GetAxisRaw("Vertical"));

        if (move > 0.1f)
        {
            // Increase timer while moving
            timer += Time.deltaTime * wobbleSpeed;

            if (bobInstead)
            {
                // Up and down bobbing
                float y = Mathf.Sin(timer) * (wobbleAmount * 0.01f);
                transform.localPosition = startPos + new Vector3(0, y, 0);
            }
            else
            {
                // Left-right tilt
                float z = Mathf.Sin(timer) * wobbleAmount;
                transform.localEulerAngles = startRot + new Vector3(0, 0, z);
            }
        }
        else
        {
            // Smoothly reset when idle
            timer = 0f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, Time.deltaTime * 5f);
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, startRot, Time.deltaTime * 5f);
        }
    }
}
