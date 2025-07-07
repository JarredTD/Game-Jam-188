using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    Rigidbody rb;
    Animator animator;
    [SerializeField] int speed = 5;
    [SerializeField] public float speedMultiplier = 1f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float sprintSpeedMultiplier = 1f;
    [SerializeField] float inertiaMultiplier = 1.5f;
    [SerializeField] bool moving;
    [SerializeField] bool btnpressed;
    [SerializeField] public bool grounded;
    [SerializeField] float moveInput;
    [SerializeField] Vector2 lastMoveInput;
    [SerializeField] LayerMask mask;
    [SerializeField] private bool wasGrounded = true;   
    public BoxRotate boxRotate;
    bool sprintPressed;

    // Whether the player is touching either wall
    [SerializeField] bool touchLeft;
    [SerializeField] bool touchRight;

    [SerializeField] AudioSource effectsSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        effectsSource = GetComponent<AudioSource>();
    }

    #region Core Movement Methods
    private void FixedUpdate()
    {
        UpdateAnimator();
        if (!boxRotate.rotating) { GetGrounded(); }
        //GetGrounded();
        SpeedSmoothing();
        //if (speedMultiplier < 2) speedMultiplier *= sprintSpeedMultiplier;
        float targetVelocity = moveInput * speed * speedMultiplier;
        Vector3 velocity = rb.linearVelocity;
        rb.linearVelocity = new Vector3(targetVelocity, velocity.y, velocity.z);

        GetWalls(lastMoveInput);
    }
    /*
    public void OnMove(InputAction.CallbackContext context)
    {
        lastMoveInput = context.ReadValue<Vector3>();
        moveInput = lastMoveInput.x;
        moving = context.performed;
        UpdateAnimator();
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && grounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            moveInput = lastMoveInput.x;
            UpdateAnimator();
        }  
    }
    */
    #endregion

    #region Modifying Movement Methods

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            sprintPressed = true;
            sprintSpeedMultiplier = 2f;
        }
        else if (context.canceled)
        {
            sprintPressed = false;
            sprintSpeedMultiplier = 1f;
        }
    }

    void SpeedSmoothing()
    {
        if (moving && speedMultiplier < 1)
        {
            speedMultiplier += Time.deltaTime * inertiaMultiplier;
        }
        else if (!moving && speedMultiplier > 0)
        {
            speedMultiplier -= Time.deltaTime * 2;
            if (speedMultiplier < 0) speedMultiplier = 0;
        }
    }

    #endregion

    #region Checks

    void GetGrounded()
    {
        RaycastHit hitLeft;
        RaycastHit hitRight;
        RaycastHit hit;

        Vector3 playerLeft = transform.position - new Vector3(0.40f, 0, 0);
        Vector3 playerRight = transform.position + new Vector3(0.40f, 0, 0);


        grounded = Physics.Raycast(transform.position, Vector3.down, out hit, .6f, mask) ||
                   Physics.Raycast(playerLeft, Vector3.down, out hitLeft, .6f, mask) ||
                   Physics.Raycast(playerRight, Vector3.down, out hitRight, .6f, mask);
        //Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.red);
        //Debug.DrawRay(playerLeft, Vector3.down * hitLeft.distance, Color.red);
        //Debug.DrawRay(playerRight, Vector3.down * hitRight.distance, Color.red);


        if (boxRotate.rotating) { grounded = false; }
        if (!wasGrounded && grounded)
        {
            OnLand();
        }

        wasGrounded = grounded;
    }

    private void OnLand()
    {
        effectsSource.Play();
    }
    public void GetWalls(Vector3 moveInput)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.right, out hit, .6f) && moveInput.x >= 1)
        { touchRight = true; Debug.DrawRay(transform.position, Vector3.right * hit.distance, Color.red); }
        else { touchRight = false; }

        if (Physics.Raycast(transform.position, Vector3.left, out hit, .6f) && moveInput.x <= -1)
        { touchLeft = true; Debug.DrawRay(transform.position, Vector3.left * hit.distance, Color.red); }
        else { touchLeft = false; }

        UpdateAnimator();
    }

    public void UpdateAnimator()
    {
        animator.SetBool("isGrounded", grounded);
        animator.SetBool("touchRight", touchRight);
        animator.SetBool("touchLeft", touchLeft);
    }

    private void Update()
    {
        float horizontal = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            horizontal = -1f;
            btnpressed = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontal = 1f;
            btnpressed = true;
        }
        else btnpressed = false;
        lastMoveInput = new Vector3(horizontal, 0f, 0f);
        moveInput = horizontal;
        moving = Mathf.Abs(horizontal) > 0.01f;
        UpdateAnimator();
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            moveInput = lastMoveInput.x;
            UpdateAnimator();
        }
    }
    #endregion

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Gem")
        {
            boxRotate.rotatedInAir = false;
            boxRotate.playerSprite.color = Color.white;
            //boxRotate.playerSprite.color = new Color(0.3f, 0.5f, 0.7f);
            Debug.Log("gem hit");
            Destroy(collision.gameObject);
        }

        
    }
}

