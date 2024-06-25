using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class InterruptorCocina : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private GameObject destroyableObject;
    [SerializeField] private AudioSource dialogoMadre;
    [SerializeField] private AudioSource sonidoBoton;

    private bool timerActive = true;
    private bool isActive = true;

    public void UpdateDialogueObject(DialogueObject dialogueObject)
    {
        this.dialogueObject = dialogueObject;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interact") && other.TryGetComponent(out Interactor interactor))
        {
            interactor.Interactable = this;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interact") && other.TryGetComponent(out Interactor interactor))
        {
            if (interactor.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this)
            {
                interactor.Interactable = null;
            }
        }
    }
    public void Interact(Interactor interactor)
    {
        foreach (DialogueResponseEvents responseEvents in GetComponents<DialogueResponseEvents>())
        {
            if (responseEvents.DialogueObject == dialogueObject)
            {
                interactor.DialogueUI.AddResponseEvents(responseEvents.Events);
                break;
            }
        }
        if (isActive) {
        destroyableObject.SetActive(false);
            sonidoBoton.Play();
        WaitSeconds();
        interactor.DialogueUI.ShowDialogue(dialogueObject);
        dialogoMadre.Play();
        isActive = false;
        }
    }

    private IEnumerator WaitSeconds()
    {
        while (timerActive)
        {
            yield return new WaitForSeconds(1.0f);
            timerActive = false;
        }
    }
}
