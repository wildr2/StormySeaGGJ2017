using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour 
{
    private float shock_speed = 0.9f;
    public SpriteRenderer spark, sprite_r;

    private float speed;

    public bool IsDangerous()
    {
        return spark != null;
    }


    private void Awake()
    {
        if (IsDangerous())
        {
            StartCoroutine(UpdateSparks());
            speed = 1;
        }
        else
        {
            speed = 1 + Random.value;
            float size = Random.Range(6, 20);
            float alpha = 0.6f + 0.4f * (1f / (size - 6f));
            sprite_r.transform.localScale = Vector3.one * size;
            
            Color c = Color.Lerp(new Color(0.3f, 0.3f, 0.3f),
                new Color(0.2f, 0.2f, 0.2f), Random.value);
            c.a = alpha;
            sprite_r.color = c;
        }
    }
    private void Update()
    {
        transform.position = (Vector2)transform.position +
            Vector2.left * Time.deltaTime * ScrollManager.ScrollSpeed * speed;

        if (transform.position.x + sprite_r.bounds.size.x / 2f < -9)
            Destroy(gameObject);
    }

    private IEnumerator UpdateSparks()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.value * 3);

            spark.transform.position = (Vector2)transform.position
                + (Vector2)Random.onUnitSphere * Random.value * 2;
            spark.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            spark.gameObject.SetActive(false);
        }
    }
}
