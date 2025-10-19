using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Sequence")]
public class DialogueSequence : ScriptableObject
{
    [System.Serializable]
    public class Line
    {
        [TextArea(2, 4)] public string text;
        public AudioClip voiceOptional;
    }

    public Line[] lines;
}
