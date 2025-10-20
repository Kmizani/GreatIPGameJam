using System.Collections;
using TMPro;
using UnityEngine;

public class Typewriter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    public string FullText;
    [SerializeField] private float typingSpeed = 0.03f; // delay between each letter

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    public bool textFullyShown = false;

    void OnEnable()
    {
        LoreScript.nextPart += StartTyping;
        LoreScript.revealAllText += RevealAllText;
    }

    void OnDisable()
    {
        LoreScript.nextPart -= StartTyping;
        LoreScript.revealAllText -= RevealAllText;
    }

    private void StartTyping()
    {
        // Stop any current coroutine before starting new one
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        _text.text = "";
        textFullyShown = false;
        typingCoroutine = StartCoroutine(TypeText());
    }

    private void RevealAllText()
    {
        // Stop the *running instance* of the coroutine
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        _text.text = FullText;
        isTyping = false;
        textFullyShown = true;
    }

    private IEnumerator TypeText()
    {
        isTyping = true;
        foreach (char c in FullText)
        {
            _text.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        textFullyShown = true;
    }
}
