using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Llaves : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject destroyableObject;
    [SerializeField] private GameObject activableObject;
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
        destroyableObject.SetActive(false);
        activableObject.SetActive(true);
        Destroy(gameObject);
    }
}
