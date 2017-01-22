using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour 
{
    public bool background = false;

	private void Update()
    {
        transform.position = (Vector2)transform.position +
            Vector2.left * Time.deltaTime * Boat.ScrollSpeed * (background ? 0.7f : 1);
    }
}
