using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueSystemDemo : MonoBehaviour
{
    [SerializeField] private string[] dialogues;

    [SerializeField] private Text dialogueText;

    [SerializeField] private float dialogueTime;
    [SerializeField] private float nextDialogueTime; 

    public void StartDialogue()
    {
        StartCoroutine(DialogueCoroutine());
    }

    IEnumerator DialogueCoroutine()
    {
        for (int i = 0; i < dialogues.Length; i++)
        {
            dialogueText.DOText(dialogues[i], dialogueTime);
            yield return new WaitForSeconds(nextDialogueTime);
            dialogueText.text = "";
        }
    }
}
