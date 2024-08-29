using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static System.Net.WebRequestMethods;
using static Unity.Burst.Intrinsics.X86;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;
    public DialogueUI DialogueUI => dialogueUI;

    [SerializeField] private Interactor interactor;

    [SerializeField] private AudioSource footstepIzq;
    [SerializeField] private AudioSource footstepDer;
    [SerializeField] private GameObject safeZones;

    public bool footstepIzqIsPlayable = false;
    public bool footstepDerIsPlayable = false;

    [SerializeField] private float playerSpeed;
    [SerializeField] private float runMultiplier;
    [SerializeField] private float staminaMax;
    [SerializeField] private float staminaOverSecond;
    [SerializeField] private float staminaDelay;
    [SerializeField]
    private bool isInSafeZone = true;
    [SerializeField]
    private bool isInConstantSafeZone = true;

    public bool isUnderLight;

    private bool isRunning = false;

    private float staminaTimer = 0;
    private float currentStamina;
    private bool canRun = true;
    private bool canStaminaIncrease = true;

    private Animator playerAnimator;
    private Rigidbody2D playerRigidbody;

    [SerializeField] private GameObject direction;
    private Vector2 playerDirection;
    private String facingDirection = "down";
    private float lastmoveX;
    private float lastmoveY;
    private float moveX;
    private float moveY;

    public bool mustActivateSafeZone = false;
    [SerializeField]
    private SceneManager sceneManager;

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

        if (mustActivateSafeZone)
        {
            ActivateSafeZones();
        }

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

        if (dialogueUI.IsOpen)
        {
            FullStop();
            return;
        }
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

    void playFootstepDer()
    {
        if (footstepDerIsPlayable)
        {
            if (!footstepDer.isPlaying)
            {
                footstepDer.Play();
                footstepDerIsPlayable = false;
            }
        }
    }

    void playFootstepIzq()
    {
        if (footstepIzqIsPlayable)
        {
            if (!footstepIzq.isPlaying)
            {
                footstepIzq.Play();
                footstepIzqIsPlayable = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Light"))
        {
            isUnderLight = true;
        }

        if (collision.CompareTag("Monster"))
        {
            KillPlayer();
        }

        if (collision.CompareTag("SafeZone"))
        {
            isInSafeZone = true;
        }

        if (collision.CompareTag("ConstantSafeZone"))
        {
            isInConstantSafeZone = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Light"))
        {
            isUnderLight = false;
        }

        if (collision.CompareTag("SafeZone"))
        {
            isInSafeZone = false;
        }

        if (collision.CompareTag("ConstantSafeZone"))
        {
            isInConstantSafeZone = false;
        }
    }

    public bool getIsUnderLight()
    {
        return isUnderLight;
    }

    public void KillPlayer()
    {
        SceneManager.LoadScene("Act2");

    }

    public void FullStop()
    {
        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.angularVelocity = 0;

        playerRigidbody.Sleep();
        moveX = 0;
        moveY = 0;
        isRunning = false;
        playerAnimator.SetFloat("horizontal", moveX);
        playerAnimator.SetFloat("vertical", moveY);
        playerAnimator.SetFloat("lasthorizontal", lastmoveX);
        playerAnimator.SetFloat("lastvertical", lastmoveY);
        playerAnimator.SetFloat("speed", 0);
        playerAnimator.SetBool("isRunning", isRunning);
    }

    public bool getIsInSafeZone()
        {
            return isInSafeZone;
        }

    public bool getIsInConstantSafeZone()
    {
        return isInConstantSafeZone;
    }

    private void ActivateSafeZones()
    {
        safeZones.SetActive(true);
        mustActivateSafeZone = false;
    }
}