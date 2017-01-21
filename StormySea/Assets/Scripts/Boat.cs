using UnityEngine;
using System.Collections;

public class Boat : MonoBehaviour 
{
    private Ocean ocean;

    private void Awake()
    {
        ocean = FindObjectOfType<Ocean>();
    }

    private void Update()
    {
        Vector2 pos = transform.position;
        Vector2 ocean_pos = new Vector2(pos.x, ocean.GetHeight(pos.x));
        Vector2 ocean_pos_left = new Vector2(pos.x - 0.01f, ocean.GetHeight(pos.x - 0.01f));
        Vector2 tangent = (ocean_pos - ocean_pos_left).normalized;

        float slope = tangent.y / tangent.x;
        float slope_factor = slope >= 0 ? 1f / (slope + 1) : (-slope + 1);
        pos.x -= slope * 2f * Time.deltaTime;

        float input_x = Input.GetAxis("Horizontal");
        pos.x += input_x * 5f * Time.deltaTime;

        transform.position = pos;


        UpdateWaveAttachment();
    }
    private void UpdateWaveAttachment()
    {
        Vector2 pos = transform.position;

        Vector2 ocean_pos = new Vector2(pos.x, ocean.GetHeight(pos.x));
        Vector2 ocean_pos_left = new Vector2(pos.x-0.01f, ocean.GetHeight(pos.x-0.01f));
        Vector2 tangent = (ocean_pos - ocean_pos_left).normalized;

        // Set Position
        Vector2 target_pos = new Vector2(pos.x, ocean_pos.y + 0.5f);
        transform.position = Vector2.Lerp(pos, target_pos, Time.deltaTime * 20f);

        // Set Rotation
        Quaternion target_r = Quaternion.Euler(0, 0,
            Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg);

        transform.rotation = Quaternion.Slerp(transform.rotation, target_r, Time.deltaTime * 4f);
    }
}
