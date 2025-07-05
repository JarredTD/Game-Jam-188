using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody rb;

    [Header("Information")]
    [SerializeField] private Vector3 pos = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 velocity = new Vector3(0, 0, 0);

    [Header("Environment")]
    [SerializeField] private float envScale = 0.1f; // Relative scale of physics engine
    [SerializeField] private float maxSpeed = 1f;
    [SerializeField] private float maxFallSpeed = 1f;
    [SerializeField] private float gravity = -10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pos = transform.position;
    }


    void Update()
    {
        DoPhysics();
    }

    void DoPhysics()
    {
        // Physics math ===========================================================

        // Gravity calc
        float yVel = velocity.y;
        if (yVel > -maxFallSpeed) // Player isn't at max fall speed yet
        {
            yVel = gravity * Time.deltaTime;
            if (yVel < -maxFallSpeed) { yVel = maxFallSpeed; } // Makes sure gravity doesn't overflow
        }
       


        velocity.y = yVel;

        // Move player ===========================================================
        pos += velocity * Time.deltaTime * envScale;

        transform.position = pos;

    }
}
