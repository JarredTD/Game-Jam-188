using UnityEngine;

public class AlwaysUpSprite : MonoBehaviour
{
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = Vector3.up;
    }
}
