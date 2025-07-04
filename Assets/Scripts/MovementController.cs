using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] int speed = 5;
    [SerializeField] float speedMultiplier = 1f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float sprintSpeedMultiplier = 1f;
    [SerializeField] float inertiaMultiplier = 1f;
    [SerializeField] bool moving;
    float moveInput;
    bool sprintPressed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    #region Core Movement Methods
    private void FixedUpdate()
    {
        SpeedSmoothing();
        if(speedMultiplier < 2) speedMultiplier *= sprintSpeedMultiplier;
        float targetVelocity = moveInput * speed * speedMultiplier;
        rb.linearVelocity = new Vector2(targetVelocity, rb.linearVelocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().x;
        moving = context.performed;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
        else if(!moving && speedMultiplier > 0)
        {
            speedMultiplier -= Time.deltaTime * 2;
            if (speedMultiplier < 0) speedMultiplier = 0;
        }
    }

    #endregion


}

