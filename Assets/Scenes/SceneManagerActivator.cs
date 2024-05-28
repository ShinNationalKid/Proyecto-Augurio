using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerActivator : MonoBehaviour
{
    [SerializeField] private string scene;

    public void ChangeScene()
    {
        SceneManager.LoadScene(scene);
    }
}