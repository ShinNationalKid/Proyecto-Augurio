using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventDialogue : MonoBehaviour
{
    [SerializeField]
    private DialogueObject dialogueObject;

    [SerializeField] private DialogueUI dialogueUI;
    public DialogueUI DialogueUI => dialogueUI;

    [SerializeField]
    private bool isOneTime;

    private bool activated;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!isOneTime)
        {
            StartDialogue();
        }
        else 
        {
            if (!activated)
            {
                activated = true;
                StartDialogue();
            }
        }
    }
    private void StartDialogue()
    {
        dialogueUI.ShowDialogue(dialogueObject);
    }
}
