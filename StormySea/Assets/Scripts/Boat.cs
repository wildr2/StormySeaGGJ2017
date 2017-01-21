using UnityEngine;
using System.Collections;

public class Boat : MonoBehaviour 
{
    private Ocean ocean;
    private Rigidbody2D rb;
    public float speed = 3f;
    private float slope_speed = 50f;
    private float velocity;


    private void Awake()
    {
        ocean = FindObjectOfType<Ocean>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
    }
    private void FixedUpdate()
    {
        UpdateMovement();
    }
    private void UpdateMovement()
    {
        Vector2 pos = rb.position;
        Vector2 ocean_pos = new Vector2(pos.x, ocean.GetHeight(pos.x));
        Vector2 ocean_pos_left = new Vector2(pos.x - 0.01f, ocean.GetHeight(pos.x - 0.01f));
        Vector2 tangent = (ocean_pos - ocean_pos_left).normalized;


        // Acceleration
        float acceleration = 0;

        // input
        float input_x = Input.GetAxis("Horizontal");
        acceleration += input_x * speed;

        // slope
        float slope = tangent.y / tangent.x;
        float slope_factor = slope >= 0 ? 1f / (slope * slope_speed + 1) : (-slope * slope_speed + 1);
        acceleration -= slope * 1f;

        // velocity, position
        velocity += acceleration * Time.deltaTime;
        velocity /= 1 + (1.5f * Time.deltaTime);
        Vector2 newpos = pos + Vector2.right * velocity * Time.deltaTime;
        newpos.y = ocean_pos.y;
        rb.MovePosition(newpos);

        // Rotation
        //if (pos.y <= ocean_pos.y)
        //{
            float target_r = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;
            rb.MoveRotation(Mathf.Lerp(rb.rotation, target_r, Time.deltaTime * 8f));
        //}
    }
}
