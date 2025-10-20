using UnityEngine;

[RequireComponent(typeof(Collider))]
public class NPCConversation : MonoBehaviour
{
    [Header("Who can talk")]
    [SerializeField] private Transform player;       // drag  player
    [SerializeField] private float talkRadius = 2.0f;

    [Header("Indicators")]
    [SerializeField] private GameObject exclamationWorld; // worldspace "!"
    [SerializeField] private DialogueUI ui;                // scene UI controller

    [Header("Dialogue Sets")]
    [SerializeField] private DialogueSequence initialDialogue;
    [SerializeField] private DialogueSequence afterMissionDialogue;

    private DialogueSequence activeDialogue;
    private bool playerInside;

    void Awake()
    {
        if (!player)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }
        activeDialogue = initialDialogue;
        if (ui) ui.ShowPressE(false);
    }

    void Update()
    {
        if (!player || !ui || ui.IsRunning) return;

        // proximity check
        float dist = Vector3.Distance(player.position, transform.position);
        bool near = dist <= talkRadius;

        if (near && exclamationWorld && exclamationWorld.activeSelf)
            ui.ShowPressE(true);
        else
            ui.ShowPressE(false);

        // interact
        if (near && exclamationWorld && exclamationWorld.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            StartConversation();
        }
    }

    void StartConversation()
    {
        if (!activeDialogue || !ui) return;

        // hide indicators for this NPC while talking
        if (exclamationWorld) exclamationWorld.SetActive(false);
        ui.ShowPressE(false);

        ui.StartDialogue(activeDialogue, onDone: () =>
        {
            // done talking  do nothing here exclamation stays OFF
        });
    }

    // External hooks to call from quest scripts 

    public void EnableExclamation(bool on)
    {
        if (exclamationWorld) exclamationWorld.SetActive(on);
    }

    public void SetDialogue(DialogueSequence newSeq)
    {
        activeDialogue = newSeq;
    }

    public void SwitchToAfterMissionDialogueAndEnableMark()
    {
        activeDialogue = afterMissionDialogue ? afterMissionDialogue : activeDialogue;
        EnableExclamation(true);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, talkRadius);
    }
}
