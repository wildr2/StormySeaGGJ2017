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
        
        float height0 = ocean.GetHeight(pos.x-0.1f);
        float height1 = ocean.GetHeight(pos.x);
        Vector2 tangent = new Vector2(pos.x, height1) - new Vector2(pos.x-0.1f, height0);
        tangent = tangent.normalized;

        pos.y = height1 + 0.5f;

        transform.position = pos;
        transform.rotation = Quaternion.Euler(0, 0, 
            Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg);


    }
}
