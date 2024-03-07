using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;

    public void UpdateDialogueObject(DialogueObject dialogueObject)
    {
        this.dialogueObject = dialogueObject;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Interact") && other.TryGetComponent(out Interactor interactor))
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
            if ( responseEvents.DialogueObject == dialogueObject)
            {
                interactor.DialogueUI.AddResponseEvents(responseEvents.Events);
                break;
            }
        }
        interactor.DialogueUI.ShowDialogue(dialogueObject);
    }
}
