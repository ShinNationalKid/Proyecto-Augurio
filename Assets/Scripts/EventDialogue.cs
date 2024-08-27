using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventDialogue : MonoBehaviour
{
    [SerializeField]
    private DialogueObject dialogueObject;

    [SerializeField]
    private Player player;

    [SerializeField] private DialogueUI dialogueUI;
    public DialogueUI DialogueUI => dialogueUI;

    [SerializeField]
    private bool isOneTime;

    private bool activated;

    private bool canBeActivated=true;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canBeActivated) { 
            if (isOneTime)
            {
                StartDialogue();
                canBeActivated = false;
            }
            else 
            {
                if (!dialogueUI.IsOpen)
                {
                    activated = false;
                }
                if (!activated)
                {
                    activated = true;
                    StartDialogue();
                }
            }
        }
    }
    private void StartDialogue()
    {
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        player.FullStop();
        dialogueUI.ShowDialogue(dialogueObject);
    }
}
