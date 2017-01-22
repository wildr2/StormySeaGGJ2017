using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour 
{
	private void Update()
    {
        transform.position = (Vector2)transform.position +
            Vector2.left * Time.deltaTime * Boat.ScrollSpeed;
    }
}
