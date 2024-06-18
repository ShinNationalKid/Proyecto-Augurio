using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Teleporter : MonoBehaviour
{
    public Image black;
    public Animator anim;

    [SerializeField] private GameObject spawn;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetTrigger("Teleport");
            other.transform.SetPositionAndRotation(spawn.transform.position, other.transform.rotation);
        }
    }
}
