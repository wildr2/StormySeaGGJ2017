using UnityEngine;
using System.Collections;

public class RainManager : MonoBehaviour 
{
    private SpriteRenderer[] sprites;
    private float speed = 50;

	private void Awake()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
    }
    private void Update()
    {
        for (int i = 0; i < sprites.Length; ++i)
        {
            sprites[i].transform.position = sprites[i].transform.position +
                -sprites[i].transform.up * Time.deltaTime * speed;
        }

        for (int i = 0; i < sprites.Length; ++i)
        {
            float dist = Vector2.Distance(sprites[i].transform.position, transform.position);
            float sprite_height = sprites[i].sprite.bounds.size.y * sprites[i].transform.localScale.y;

            if (dist > sprite_height * 2f)
            {
                

                int index = (i + 1) % sprites.Length;
                sprites[i].transform.position = sprites[index].transform.position +
                    sprites[i].transform.up * sprite_height;
            }
        }
    }
}
