using UnityEngine;

public class KillZResponder : MonoBehaviour
{
    [SerializeField] private NPCConversation targetNPC;
    [SerializeField] private bool useAfterMissionDialogue = true;
    [SerializeField] private DialogueSequence customDialogueIfNotUsingAfter;
    [SerializeField] private bool reEnableExclamation = true;
    [SerializeField] private bool destroySelf = true;
    [SerializeField] private float destroyDelay = 0f;
    [SerializeField] private bool onlyOnce = true;

    private bool fired;

    public void HandleKillZ(KillZVolume source)
    {
        if (onlyOnce && fired) return;
        fired = true;

        if (targetNPC)
        {
            if (useAfterMissionDialogue)
                targetNPC.SwitchToAfterMissionDialogueAndEnableMark();
            else
            {
                if (customDialogueIfNotUsingAfter) targetNPC.SetDialogue(customDialogueIfNotUsingAfter);
                if (reEnableExclamation) targetNPC.EnableExclamation(true);
            }
        }

        if (destroySelf) Destroy(gameObject, destroyDelay);
    }
}
