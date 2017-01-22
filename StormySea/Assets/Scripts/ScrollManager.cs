using UnityEngine;
using System.Collections;

public class ScrollManager : MonoBehaviour
{
    private float normal_scroll_speed = 2;

    public static float ScrollSpeed { get; private set; }
    public bool Paused { get; private set; }
    public float DistanceTravelled { get; private set; }

    public void Play()
    {
        ScrollSpeed = normal_scroll_speed;
        Paused = false;
    }
    public void Pause()
    {
        ScrollSpeed = 0;
        Paused = true;
    }

    private void Start()
    {
        Play();
    }
    private void Update()
    {
        DistanceTravelled += ScrollSpeed * Time.deltaTime;
    }
}
