using UnityEngine;
using System.Collections;

public class Boat : MonoBehaviour 
{
    private Ocean ocean;
    private Rigidbody2D rb;

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
        //UpdateWaveAttachment();
        UpdateMovement();
    }
    private void UpdateMovement()
    {
        Vector2 pos = rb.position;
        Vector2 ocean_pos = new Vector2(pos.x, ocean.GetHeight(pos.x));
        Vector2 ocean_pos_left = new Vector2(pos.x - 0.01f, ocean.GetHeight(pos.x - 0.01f));
        Vector2 tangent = (ocean_pos - ocean_pos_left).normalized;


        // Position
        Vector2 target_pos = new Vector2(pos.x, ocean_pos.y - 0.5f);

        float slope = tangent.y / tangent.x;
        float slope_factor = slope >= 0 ? 1f / (slope + 1) : (-slope + 1);
        target_pos.x -= slope * 2f * Time.deltaTime;

        float input_x = Input.GetAxis("Horizontal");
        target_pos.x += input_x * 5f * Time.deltaTime;

        rb.MovePosition(target_pos);


        // Rotation
        if (pos.y <= ocean_pos.y)
        {
            float target_r = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;
            rb.MoveRotation(Mathf.Lerp(rb.rotation, target_r, Time.deltaTime * 4f));
        }
    }
    private void UpdateWaveAttachment()
    {
        Vector2 pos = rb.position;
        Vector2 ocean_pos = new Vector2(pos.x, ocean.GetHeight(pos.x));
        Vector2 ocean_pos_left = new Vector2(pos.x - 0.01f, ocean.GetHeight(pos.x - 0.01f));
        Vector2 tangent = (ocean_pos - ocean_pos_left).normalized;

        // Move position
        Vector2 target_pos = new Vector2(pos.x, ocean_pos.y - 0.5f);
        rb.MovePosition(Vector2.Lerp(pos, target_pos, Time.deltaTime * 20f));

        // Move Rotation
        float target_r = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;
        rb.MoveRotation(Mathf.Lerp(rb.rotation, target_r, Time.deltaTime * 4f));
    }
}
