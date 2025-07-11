using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class BoxRotate : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 1;
    [SerializeField] public bool rotating = false;
    [SerializeField] private float speed = 1;
    [SerializeField] private float moveInput;
    [SerializeField] public bool isRotated90Or270;
    [SerializeField] public bool rotatedInAir;
    public MovementController movementController;
    private float rotateTarget = 0;

    [SerializeField] private GameObject player;
    [SerializeField] public SpriteRenderer playerSprite;
    void OnAwake()
    {
        
    }

    private void Start()
    {
        playerSprite = player.GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (rotating)
        {
            player.GetComponent<MovementController>().speedMultiplier = 0;
            player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

            transform.Rotate(0, 0, speed * Time.deltaTime);

            rotateTarget -= Mathf.Abs(speed * Time.deltaTime);
            if (rotateTarget < 0)
            {
                rotating = false;
                transform.eulerAngles = new Vector3(
                    transform.eulerAngles.x,
                    transform.eulerAngles.y,
                    Mathf.Round(transform.eulerAngles.z / 90) * 90
                );
            }
        }

        // Check if the box is rotated 90 or 270 degrees (modulo 360)
        float z = Mathf.Round(transform.eulerAngles.z) % 360;
        isRotated90Or270 = (Mathf.Approximately(z, 90f) || Mathf.Approximately(z, 270f));
        if(movementController.grounded && !rotating)
        {
            rotatedInAir = false;
            playerSprite.color = Color.white;
        }
        
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().x;
        if (!rotating && context.performed && !rotatedInAir)
        {
            if (moveInput != 0)
            {
                rotating = true;
                speed = -moveInput * rotateSpeed;
                rotateTarget = 90;
                rotatedInAir = true;
                playerSprite.color = new Color(0.3f, 0.5f, 0.7f);

                movementController.grounded = false;
            }
        }
    }
}
