using UnityEngine;
using System.Collections;

public class Ocean : MonoBehaviour
{
    // Waves
    private CompositeWave wave;

    // Graphics
    public LineRenderer line;
    private int vertices_n = 1000;
    private int line_length = 35;


    public float GetHeight(float x)
    {
        float t = (x + line_length / 2f) / line_length;
        return wave.GetHeight(t);
    }


    private void Awake()
    {
        wave = new CompositeWave();
        wave.waves = new MovingWave[3];
        wave.waves[0] = new MovingWave(1, 50, 2);
        wave.waves[1] = new MovingWave(1, 30, -1);
        wave.waves[2] = new MovingWave(0.5f, 10, 3);

        line = GetComponent<LineRenderer>();
        line.numPositions = vertices_n;
    }
    private void Update()
    {
        RedrawWave();
    }
    private void RedrawWave()
    {
        for (int i = 0; i < vertices_n; ++i)
        {
            float t = (float)i / vertices_n;

            Vector2 vertex = new Vector2();
            vertex.x = t * line_length - line_length / 2f;
            vertex.y = wave.GetHeight(t) - line.startWidth;

            line.SetPosition(i, vertex);
        }
    }
}

public class MovingWave
{
    public float amplitude;
    public float frequency;
    public float speed;

    public MovingWave(float a, float f, float s)
    {
        amplitude = a;
        frequency = f;
        speed = s;
    }

    /// <summary>
    /// t from 0 to 1
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public float GetHeight(float t)
    {
        return Mathf.Sin(frequency * t + Time.time * speed) * amplitude;
    }
}

public class CompositeWave
{
    public MovingWave[] waves;

    public float GetHeight(float t)
    {
        float h = 0;
        foreach (MovingWave wave in waves)
        {
            h += wave.GetHeight(t);
        }
        return h;
    }
}
