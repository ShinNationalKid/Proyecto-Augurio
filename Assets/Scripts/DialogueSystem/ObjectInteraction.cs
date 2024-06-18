using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject activeObject;

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
            interactor.Interactable = null;
        }
    }
    public void Interact(Interactor interactor)
    {
        activeObject.SetActive(false);
        Debug.Log("hola");
    }
}
