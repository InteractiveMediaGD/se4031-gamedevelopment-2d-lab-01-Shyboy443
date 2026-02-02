using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    public float speed = 3f;
    public float minY = -4f;
    public float maxY = 4f;
    public float minX = -8f;
    public float maxX = 8f;
    
    private Vector2 direction;

    void Start()
    {
        // Random initial direction
        direction = Random.insideUnitCircle.normalized;
    }

    void Update()
    {
        // Move obstacle
        transform.Translate(direction * speed * Time.deltaTime);
        
        // Bounce off boundaries
        Vector3 pos = transform.position;
        
        if (pos.x <= minX || pos.x >= maxX)
        {
            direction.x = -direction.x;
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
        }
        
        if (pos.y <= minY || pos.y >= maxY)
        {
            direction.y = -direction.y;
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
        }
        
        transform.position = pos;
    }
}
