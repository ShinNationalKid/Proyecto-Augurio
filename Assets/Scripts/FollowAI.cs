using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;

public class FollowAI: MonoBehaviour
{
    public GameObject target; //the enemy's target
    public float moveSpeed = 10; //move speed
    public float rotationSpeed = 20; //speed of turning
    [SerializeField]
    private GameObject direction;
    private String facingDirection = "s";
    private Rigidbody2D rb;

    private Vector2 objectDirection;
    private Animator objectAnimator;

    private float currentDirectionX;
    private float currentDirectionY;
    private float previousDirectionX;
    private float previousDirectionY;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        objectAnimator = GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        //rotate to look at the player
        direction.transform.rotation = Quaternion.Slerp(direction.transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), rotationSpeed * Time.deltaTime);
        //direction.transform.rotation = Quaternion.Euler(0f, direction.transform.rotation.x, direction.transform.rotation.y);
        //move towards the player
        transform.position += direction.transform.forward * Time.deltaTime * moveSpeed;


        currentDirectionX = (transform.position.x - previousDirectionX) / Time.deltaTime;
        currentDirectionY = (transform.position.y - previousDirectionY) / Time.deltaTime;

        objectDirection = new Vector2(currentDirectionX, currentDirectionY).normalized;
        objectAnimator.SetFloat("horizontal", direction.transform.forward.x);
        objectAnimator.SetFloat("vertical", direction.transform.forward.y);
        objectAnimator.SetFloat("speed", objectDirection.sqrMagnitude);



        previousDirectionX = currentDirectionX;
        previousDirectionY = currentDirectionY;
    }

    void FacingDirection()
    {
        float n = Vector2.Distance(objectDirection, new Vector2(0.0f, 1.0f));
        float e = Vector2.Distance(objectDirection, new Vector2(1.0f, 0.0f));
        float s = Vector2.Distance(objectDirection, new Vector2(0.0f, -1.0f));
        float o = Vector2.Distance(objectDirection, new Vector2(-1.0f, 0.0f));
        float no = Vector2.Distance(objectDirection, new Vector2(-0.75f, -0.75f));
        float ne = Vector2.Distance(objectDirection, new Vector2(0.75f, -0.75f));
        float so = Vector2.Distance(objectDirection, new Vector2(-0.75f, 0.75f));
        float se = Vector2.Distance(objectDirection, new Vector2(0.75f, 0.75f));


        if (n <= e && n <= s && n <= o && n <= no && n <= ne && n <= so && n <= se) facingDirection = "n";
        if (e <= n && e <= s && e <= o && e <= no && e <= ne && e <= so && e <= se) facingDirection = "e";
        if (s <= e && s <= n && s <= o && s <= no && s <= ne && s <= so && s <= se) facingDirection = "s";
        if (o <= e && o <= s && o <= n && o <= no && o <= ne && o <= so && o <= se) facingDirection = "o";
        if (no <= e && no <= s && no <= o && no <= n && no <= ne && no <= so && no <= se) facingDirection = "no";
        if (ne <= e && ne <= s && ne <= o && ne <= no && ne <= n && ne <= so && ne <= se) facingDirection = "ne";
        if (so <= e && so <= s && so <= o && so <= no && so <= ne && so <= n && so <= se) facingDirection = "so";
        if (se <= e && se <= s && se <= o && se <= no && se <= ne && se <= so && se <= n) facingDirection = "se";


        switch (facingDirection)
        {
            case "n":
                direction.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                break;
            case "o":
                direction.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
                break;
            case "e":
                direction.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
                break;
            case "s":
                direction.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                break;
            case "no":
                direction.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -45.0f);
                break;
            case "ne":
                direction.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 45.0f);
                break;
            case "so":
                direction.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -135.0f);
                break;
            case "se":
                direction.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 135.0f);
                break;
            default:
                break;
        }
    }
}