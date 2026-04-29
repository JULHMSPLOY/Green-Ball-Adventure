using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public float distance = 1.5f; // ระยะทางขยับ
    public float speed = 0.5f;    // ความเร็ว

    private Vector2 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // ขยับซ้าย-ขวาอัตโนมัติ
        float newX = startPos.x + Mathf.Sin(Time.time * speed) * distance;
        transform.position = new Vector2(newX, transform.position.y);
    }
}
