using UnityEngine;
using System.Collections;

public class CloudSpawner : MonoBehaviour
{
    public Cloud cloud_prefab;
    private ScrollManager scroller;

    private void Awake()
    {
        scroller = FindObjectOfType<ScrollManager>();

        Prewarm();
        StartCoroutine(UpdateSpawner());
    }
	private IEnumerator UpdateSpawner()
    {
        while (true)
        {
            while (scroller.Paused)
            {
                yield return null;
            }

            Spawn();

            yield return new WaitForSeconds(Random.value * 2f);
        }
    }

    private void Prewarm()
    {
        for (int i = 0; i < 10; ++i)
        {
            Cloud c = Spawn();
            Vector2 pos = c.transform.position;
            pos.x = Random.Range(-9, 9);
            c.transform.position = pos;
        }
    }
    private Cloud Spawn()
    {
        Cloud c = Instantiate(cloud_prefab);
        c.transform.SetParent(transform);
        c.transform.position = new Vector2(
            transform.position.x + c.sprite_r.bounds.size.x,
            transform.position.y + (Random.value - 0.5f) * 5f);

        return c;
    }

}
