using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class BoxRotate : MonoBehaviour
{
    public float rotateSpeed = 1;
    public bool rotating = false;
    private float speed = 1;
    private float rotateTarget = 0;
    void OnAwake()
    {
    }

    void Update()
    {
        if (rotating)
        {
            
            rotateTarget -= Mathf.Abs(speed * Time.deltaTime);
            if (rotateTarget < 0) rotating = false;

            transform.Rotate(0, 0, speed * Time.deltaTime);
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
