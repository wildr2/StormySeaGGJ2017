using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour 
{
    private float shock_speed = 0.9f;
    public SpriteRenderer spark, sprite;

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
            speed = 1;
        }
    }
    private void Update()
    {
        transform.position = (Vector2)transform.position +
            Vector2.left * Time.deltaTime * Boat.ScrollSpeed * speed;
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
