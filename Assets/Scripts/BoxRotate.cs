using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class BoxRotate : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 1;
    [SerializeField] private bool rotating = false;
    [SerializeField]private float speed = 1;
    [SerializeField]private float moveInput;
    private float rotateTarget = 0;

    [SerializeField] private GameObject player;
    void OnAwake()
    {

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
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().x;
        if (!rotating && context.performed)
        {
            
            if (moveInput != 0)
            {
                rotating = true;
                speed = moveInput * rotateSpeed;
                rotateTarget = 90;
            }
        }
    }
}
