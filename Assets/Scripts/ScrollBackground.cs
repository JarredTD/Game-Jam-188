using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
    private float timer;
    [SerializeField] private float speed;
    [SerializeField] private float maxScroll;
    private Vector3 pos;

    void Start()
    {
        pos = transform.position;
    }

    void Update()
    {
        timer += Time.deltaTime;
        timer = timer % maxScroll;

        transform.position = new Vector3(pos.x + timer, pos.y, pos.z);

    }
}
