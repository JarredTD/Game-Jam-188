using UnityEngine;
using System.Collections;

public class BoxRotate : MonoBehaviour
{

    void OnAwake()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.Rotate(0, 0, 90);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.Rotate(0, 0, -90);
        }
    }
}
