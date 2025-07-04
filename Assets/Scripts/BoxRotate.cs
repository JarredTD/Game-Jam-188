using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class BoxRotate : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 1;
    [SerializeField] private bool rotating = false;
    private float speed = 1;
    private float rotateTarget = 0;

    [SerializeField] private GameObject player;
    void OnAwake()
    {

    }

    void Update()
    {
        if (rotating)
        {
            player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

            transform.Rotate(0, 0, speed * Time.deltaTime);

            rotateTarget -= Mathf.Abs(speed * Time.deltaTime);
            if (rotateTarget < 0)
            {
                rotating = false;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, (Mathf.Round(transform.eulerAngles.z / 90) * 90));
            }

            
        }
        else
        {
            if (Input.GetKey(KeyCode.Q))
            {
                rotating = true;
                speed = 1 * rotateSpeed;
                rotateTarget = 90;
                //transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                rotating = true;
                speed = -1 * rotateSpeed;
                rotateTarget = 90;
                //transform.Rotate(0, 0, -rotateSpeed * Time.deltaTime);
            }
        }

        
    }
}
