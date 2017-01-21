using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour 
{
    private float shock_speed = 1.3f;

    private void Update()
    {
        transform.position = (Vector2)transform.position +
            Vector2.left * Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        Boat boat = collider.GetComponentInParent<Boat>();
        
        if (boat != null)
        {
            if (boat.BuildShock(Time.deltaTime * shock_speed))
            {

            }
        }
    }
}
