using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [Header("Bottom Banner")]
    [SerializeField] private RectTransform banner;     
    [SerializeField] private float hiddenY = -160f;
    [SerializeField] private float shownY = 20f;
    [SerializeField] private float slideSeconds = 0.25f;
    [SerializeField] private AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Typewriter")]
    [SerializeField] private TextMeshProUGUI textLabel;
    [SerializeField, Range(1f, 120f)] private float charsPerSecond = 45f;

    [Header("Prompt")]
    [SerializeField] private GameObject pressEPrompt;  

    [Header("Audio (optional)")]
    [SerializeField] private AudioSource voiceSource;  

    public bool IsRunning { get; private set; }

    public void ShowPressE(bool show)
    {
        if (pressEPrompt) pressEPrompt.SetActive(show && !IsRunning);
    }

    public void StartDialogue(DialogueSequence seq, System.Action onDone = null)
    {
        if (!IsRunning && seq && seq.lines != null && seq.lines.Length > 0)
            StartCoroutine(Run(seq, onDone));
    }

    IEnumerator Run(DialogueSequence seq, System.Action onDone)
    {
        IsRunning = true;
        textLabel.text = string.Empty;
        ShowPressE(false);

        float prevScale = Time.timeScale;
        Time.timeScale = 0f; // freeze gameplay
        if (voiceSource) voiceSource.ignoreListenerPause = true;

        // Slide up 
        yield return Slide(hiddenY, shownY);

        for (int i = 0; i < seq.lines.Length; i++)
        {
            var line = seq.lines[i];

            textLabel.text = string.Empty;

            
            if (voiceSource)
            {
                voiceSource.Stop();
                if (line.voiceOptional)
                {
                    voiceSource.clip = line.voiceOptional;
                    voiceSource.Play();
                }
            }

            // Type out line if player presse, finish instantly 
            yield return TypeLineWithSkipUnscaled(line.text);

            //  require a press to advance to the next line
            yield return WaitForFreshAdvance();
        }

        if (voiceSource) voiceSource.Stop();
        // Slide down and unfreeze
        yield return Slide(shownY, hiddenY);
        Time.timeScale = prevScale;

        textLabel.text = string.Empty;   // leave UI blank between conversations

        IsRunning = false;
        onDone?.Invoke();
    }

    IEnumerator Slide(float fromY, float toY)
    {
        Vector2 p = banner.anchoredPosition;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / Mathf.Max(0.0001f, slideSeconds);
            float k = ease.Evaluate(Mathf.Clamp01(t));
            banner.anchoredPosition = new Vector2(p.x, Mathf.LerpUnclamped(fromY, toY, k));
            yield return null;
        }
        banner.anchoredPosition = new Vector2(p.x, toY);
    }

    

    
    IEnumerator TypeLineWithSkipUnscaled(string s)
    {
        textLabel.text = "";
        if (string.IsNullOrEmpty(s)) yield break;

        float cps = Mathf.Max(1f, charsPerSecond);
        float t = 0f;
        int shown = 0;

        while (shown < s.Length)
        {
            if (AdvancePressedThisFrame())
            {
                textLabel.text = s;         // finish instantly
                yield return WaitForRelease(); // this press prevents auto advance
                yield break;
            }

            t += Time.unscaledDeltaTime * cps;
            int next = Mathf.Clamp(Mathf.FloorToInt(t), 0, s.Length);
            if (next != shown)
            {
                textLabel.text = s.Substring(0, next);
                shown = next;
            }
            yield return null;
        }
    }

    
    IEnumerator WaitForFreshAdvance()
    {
        // Ensure we aren't holding the buttons from before
        yield return WaitForRelease();

        // Wait for a new press E or Left Click
        while (!AdvancePressedThisFrame())
            yield return null;

        //  wait until released so it doesn't chainskip
        yield return WaitForRelease();
    }

    IEnumerator WaitForRelease()
    {
        while (Input.GetKey(KeyCode.E) || Input.GetMouseButton(0))
            yield return null;

        // One extra frame to avoid sameframe retriggers
        yield return null;
    }

    bool AdvancePressedThisFrame()
    {
        return Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0);
    }
}
