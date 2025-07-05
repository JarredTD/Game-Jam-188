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
    [SerializeField] float inertiaMultiplier = 1f;
    [SerializeField] bool moving;
    [SerializeField] public bool grounded;
    [SerializeField]float moveInput;
    bool sprintPressed;

    // Whether the player is touching either wall
    private bool touchLeft = false;
    private bool touchRight = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    #region Core Movement Methods
    private void FixedUpdate()
    {
        UpdateAnimator();
        GetGrounded();
        SpeedSmoothing();
        if (speedMultiplier < 2) speedMultiplier *= sprintSpeedMultiplier;
        float targetVelocity = moveInput * speed * speedMultiplier;
        Vector3 velocity = rb.linearVelocity;
        rb.linearVelocity = new Vector3(targetVelocity, velocity.y, velocity.z);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().x;
        moving = context.performed;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && grounded)
        {
          rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }  
        }
        
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

    void GetGrounded()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, .6f))
        {
            Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.red);
            grounded = true;
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.down * 1000, Color.white);
            grounded = false;
        }
    }
    
    // Get wall collision
    public void GetWalls(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.right, out hit, .6f) && context.ReadValue<Vector2>().x >= 1)
        { touchRight = true; Debug.DrawRay(transform.position, Vector3.right * hit.distance, Color.red); }
        else { touchRight = false; }

        if (Physics.Raycast(transform.position, Vector3.left, out hit, .6f) && context.ReadValue<Vector2>().x <= -1)
        { touchLeft = true; Debug.DrawRay(transform.position, Vector3.left * hit.distance, Color.red); }
        else { touchLeft = false; }

    }

    // Updates animator information
    void UpdateAnimator()
    {
        animator.SetBool("isGrounded", grounded);
        animator.SetBool("touchRight", touchRight);
        animator.SetBool("touchLeft", touchLeft);
    }
}

