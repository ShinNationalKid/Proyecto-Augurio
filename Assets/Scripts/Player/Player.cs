using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static System.Net.WebRequestMethods;
using static Unity.Burst.Intrinsics.X86;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;
    public DialogueUI DialogueUI => dialogueUI;

    [SerializeField] private Interactor interactor;

    [SerializeField] private float playerSpeed;
    private Animator playerAnimator;
    private Rigidbody2D playerRigidbody;
    [SerializeField] private GameObject direction;
    private Vector2 playerDirection;
    private String facingDirection = "down";
    private float lastmoveX;
    private float lastmoveY;
    private float moveX;
    private float moveY;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueUI.IsOpen) return;
        //if (combatUI.IsOpen) return;

        //Processing inputs
        ProcessInputs();

        //Sending animations parameters
        playerAnimator.SetFloat("horizontal", moveX);
        playerAnimator.SetFloat("vertical", moveY);
        playerAnimator.SetFloat("lasthorizontal", lastmoveX);
        playerAnimator.SetFloat("lastvertical", lastmoveY);
        playerAnimator.SetFloat("speed", playerDirection.sqrMagnitude);
    }

    void FixedUpdate()
    {
        //Physics
        playerRigidbody.MovePosition(playerRigidbody.position + playerDirection * playerSpeed * Time.fixedDeltaTime);
    }

    void ProcessInputs()
    {
        if (MathF.Abs(Input.GetAxisRaw("Horizontal")) >= 0.1f || MathF.Abs(Input.GetAxisRaw("Vertical")) >= 0.1f)
        {
            lastmoveX = Input.GetAxisRaw("Horizontal");
            lastmoveY = Input.GetAxisRaw("Vertical");
        }
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        playerDirection = new Vector2(moveX, moveY).normalized;

        if (playerDirection.x != 0.0f || playerDirection.y != 0.0f)
        {
            FacingDirection();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            interactor.TryInteraction();
        }

        if (dialogueUI.IsOpen)
        {
            playerDirection = new Vector2(0f, 0f).normalized;
            playerAnimator.SetFloat("speed", playerDirection.sqrMagnitude);
        }
    }

    void FacingDirection()
    {
        float n = Vector2.Distance(playerDirection, new Vector2(0.0f, 1.0f));
        float e = Vector2.Distance(playerDirection, new Vector2(1.0f, 0.0f));
        float s = Vector2.Distance(playerDirection, new Vector2(0.0f, -1.0f));
        float o = Vector2.Distance(playerDirection, new Vector2(-1.0f, 0.0f));
        float no = Vector2.Distance(playerDirection, new Vector2(-0.75f, -0.75f));
        float ne = Vector2.Distance(playerDirection, new Vector2(0.75f, -0.75f));
        float so = Vector2.Distance(playerDirection, new Vector2(-0.75f, 0.75f));
        float se = Vector2.Distance(playerDirection, new Vector2(0.75f, 0.75f));


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