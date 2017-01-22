using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour 
{
    private float shock_speed = 1.3f;
    public SpriteRenderer spark, cloud;

    private void Awake()
    {
        if (spark != null) StartCoroutine(UpdateSparks());

        float r =  0.1f + Random.value * 0.2f;
        cloud.color = new Color(r, r, r, 0.5f + Random.value * 0.2f);
    }
    private void Update()
    {
        transform.position = (Vector2)transform.position +
            Vector2.left * Time.deltaTime;
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

    private void OnTriggerStay2D(Collider2D collider)
    {
        Boat boat = collider.GetComponentInParent<Boat>();
        
        if (boat != null)
        {
            boat.BuildShock(Time.deltaTime * shock_speed);
        }
    }
}
