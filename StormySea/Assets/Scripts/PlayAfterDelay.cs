using UnityEngine;
using System.Collections;

public class PlayAfterDelay : MonoBehaviour
{
    private AudioSource source;
    public float delay;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        StartCoroutine(Play());
    }
    private IEnumerator Play()
    {
        yield return new WaitForSeconds(delay);
        source.Play();
    }

}
