using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;

public class LightSource : MonoBehaviour
{
    private Light2D spotLight;
    private CircleCollider2D circleCollider;
    public float currentTime;
    private float totalTime = 10;
    public bool playerInArea;
    public bool monsterInArea;
    private float reactivateTime = 20;
    public bool isDeactivated = false;

    // Start is called before the first frame update
    void Start()
    {
        spotLight = GetComponent<Light2D>();
        circleCollider = GetComponent<CircleCollider2D>();  
        currentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (playerInArea && !isDeactivated)
        {
            currentTime = currentTime + Time.deltaTime;
            if (currentTime >= totalTime) 
            {
                spotLight.enabled = false;
                circleCollider.enabled = false;
                isDeactivated = true;
                currentTime = 0;
            }
        }

        if (isDeactivated)
        {
            currentTime = currentTime + Time.deltaTime;
            if (currentTime >= reactivateTime) 
            {
                circleCollider.enabled = true;
                spotLight.enabled = true;
                isDeactivated = false;
                currentTime = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInArea = true;
        }
        if (other.CompareTag("Monster"))
        {
            monsterInArea = true;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") )
        {
            playerInArea = false;
        }
        if (other.CompareTag("Monster"))
        {
            monsterInArea = false;
        }
    }
}
