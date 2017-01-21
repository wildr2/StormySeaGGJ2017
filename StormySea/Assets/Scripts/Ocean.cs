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
    private float line_width = 0.3f;


    public float GetHeight(float x)
    {
        float t = (x + line_length / 2f) / line_length;
        return wave.GetHeight(t) + transform.position.y;
    }


    private void Awake()
    {
        wave = new CompositeWave();
        wave.waves = new MovingWave[3];

        float amp_mult = 1.5f;
        float freq_mult = 0.7f;
        float speed_mult = 0.3f;

        wave.waves[0] = new MovingWave(1 * amp_mult, 50 * freq_mult, 2 * speed_mult);
        wave.waves[1] = new MovingWave(1 * amp_mult, 30 * freq_mult, -1 * speed_mult);
        wave.waves[2] = new MovingWave(0.5f * amp_mult, 10 * freq_mult, 3 * speed_mult);

        line = GetComponent<LineRenderer>();
        line.SetVertexCount(vertices_n);
        line.SetWidth(line_width, line_width);
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
            vertex.y = wave.GetHeight(t) - line_width / 2f;

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
