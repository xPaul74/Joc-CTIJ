using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f;      
    public float distance = 3f;   
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void FixedUpdate() 
    {
        float newY = startPos.y + Mathf.Sin(Time.fixedTime * speed) * distance;
        GetComponent<Rigidbody2D>().MovePosition(new Vector2(transform.position.x, newY));
    }


}