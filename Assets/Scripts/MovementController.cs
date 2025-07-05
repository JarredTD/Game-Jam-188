using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] int speed = 5;
    [SerializeField] public float speedMultiplier = 1f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float sprintSpeedMultiplier = 1f;
    [SerializeField] float inertiaMultiplier = 1f;
    [SerializeField] bool moving;
    [SerializeField] public bool grounded;
    [SerializeField]float moveInput;
    bool sprintPressed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    #region Core Movement Methods
    private void FixedUpdate()
    {
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

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1.1f))

        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.red);
            Debug.Log("Did Hit");
            grounded = true;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 1000, Color.white);
            Debug.Log("Did not Hit");
            grounded = false;
        }

        //grounded = true;
    }
}

