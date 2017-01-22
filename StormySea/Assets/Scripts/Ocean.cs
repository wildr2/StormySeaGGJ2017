using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ocean : MonoBehaviour
{
    // Waves
    private CompositeWave wave;

    // Graphics
    //public LineRenderer line;
    private int vertices_n = 1000;
    private int line_length = 35;
    //private float line_width = 0.3f;

    float crest_height = 0.1f;
    private Mesh water_mesh, crest_mesh;
    public MeshFilter water_meshfilter, crest_meshfilter;

    //public static float oldTime = 0;
    //public static float amplitudeModifier = 4f; //generally between 2 and 6

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

        //wave.waves[0].setAmplitude(10f);

        //line = GetComponent<LineRenderer>();
        //line.SetVertexCount(vertices_n);
        //line.SetWidth(line_width, line_width);

        InitMesh();
    }
    private void InitMesh()
    {
        water_mesh = water_meshfilter.mesh;
        RecreateWaterMesh();
        crest_mesh = crest_meshfilter.mesh;
        RecreateCrestMesh();
    }
    private void Update()
    {
        //RedrawWave();
        RecreateWaterMesh();
        RecreateCrestMesh();
        /*
        if(wave != null)
        {
        if(Time.time > oldTime + 0.1)
        {
            oldTime = Time.time;
            wave.waves[0].setAmplitude(((wave.waves[0].GetAmplitude()) + 0.01f) * 1f);
        }
        }
         */
        
    }
    private void RedrawWave()
    {
        //for (int i = 0; i < vertices_n; ++i)
        //{
        //    float t = (float)i / vertices_n;

        //    Vector2 vertex = new Vector2();
        //    vertex.x = t * line_length - line_length / 2f;
        //    vertex.y = wave.GetHeight(t) - line_width / 2f;

        //    line.SetPosition(i, vertex);
        //}
    }
    private void RecreateWaterMesh()
    {
        List<Vector3> verts = new List<Vector3>();
        for (int i = 0; i < vertices_n; ++i)
        {
            float t = (float)i / vertices_n;
            verts.Add(new Vector3(t * line_length - line_length / 2f, wave.GetHeight(t)));
        }
        for (int i = 0; i < vertices_n; ++i)
        {
            float t = (float)i / vertices_n;
            verts.Add(new Vector3(t * line_length - line_length / 2f, wave.GetHeight(t) - 10));
        }
        water_mesh.SetVertices(verts);

        List<int> tris = new List<int>();
        for (int i = 0; i < vertices_n-1; ++i)
        {
            float t = (float)i / vertices_n;
            tris.Add(i);
            tris.Add(i + vertices_n);
            tris.Add(i + vertices_n + 1);

            tris.Add(i);
            tris.Add(i + 1);
            tris.Add(i + vertices_n + 1);
        }
        water_mesh.SetTriangles(tris, 0);
        water_mesh.RecalculateNormals();
        water_mesh.RecalculateBounds();
    }
    private void RecreateCrestMesh()
    {
        List<Vector3> verts = new List<Vector3>();
        for (int i = 0; i < vertices_n; ++i)
        {
            float t = (float)i / vertices_n;
            verts.Add(new Vector3(t * line_length - line_length / 2f, wave.GetHeight(t) + crest_height));
        }
        for (int i = 0; i < vertices_n; ++i)
        {
            float t = (float)i / vertices_n;
            verts.Add(new Vector3(t * line_length - line_length / 2f, wave.GetHeight(t)));
        }
        crest_mesh.SetVertices(verts);

        List<int> tris = new List<int>();
        for (int i = 0; i < vertices_n - 1; ++i)
        {
            float t = (float)i / vertices_n;
            tris.Add(i);
            tris.Add(i + vertices_n);
            tris.Add(i + vertices_n + 1);

            tris.Add(i);
            tris.Add(i + 1);
            tris.Add(i + vertices_n + 1);
        }
        crest_mesh.SetTriangles(tris, 0);
        crest_mesh.RecalculateNormals();
        crest_mesh.RecalculateBounds();
    }
}

public class MovingWave
{
    //parameters to randomize wave needed
    public float amplitude;
    public float frequency;
    public float speed;
    //public float oldTime;
    public float amplitudeModifier; //generally between 2 and 6
    public float frequencyModifier;

    public float freqRand;
    public float freqRand2;
    public float ampRand;
    public float ampRand2;

    public MovingWave(float a, float f, float s)
    {
        amplitude = a;
        frequency = f;
        speed = s;
        //oldTime = 0;
        amplitudeModifier = 4f;
        frequencyModifier = 2.5f;

        freqRand = Random.Range(0, 4);
        freqRand2 = Random.Range(0, 4);
        ampRand = Random.Range(0, 4);
        ampRand2 = Random.Range(0, 4);
    }

    /// <summary>
    /// t from 0 to 1
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public float GetHeight(float t)
    {
        /*
       if (Time.time > oldTime + 0.05)
       {
           oldTime = Time.time;
           int randomnumber = Random.Range(0, 99);
           /*
           if (Random.value < 0.0001f)
           {
               amplitudeModifier = Random.Range(-10, 10);
           }
            */
        
        /*
           //Debug.Log(randomnumber);
           if (randomnumber > 49)
           {
               if (amplitudeModifier < 6)
               {
                   amplitudeModifier = amplitudeModifier + 0.01f;
                   Debug.Log(amplitudeModifier);
               }
           }
           else
           {
               if (amplitudeModifier > 2)
               {
                   amplitudeModifier = amplitudeModifier - 0.01f;
               }
           }
       }
        */
       amplitudeModifier = (Mathf.PerlinNoise(Time.time * ampRand * 0.1f, t * ampRand2) + 1) * 2.5f; //(0 to 1) * 2f * 4
       frequencyModifier = (((Mathf.PerlinNoise(Time.time * 0.05f * freqRand, t * freqRand2))/4) + 1.25f) * 2f * (1/(amplitudeModifier));
       //amplitudeModifier = Mathf.PerlinNoise(Time.time * ampRand * 0.1f, t * ampRand2) * 8f; //(0 to 1) * 2f * 4
       //frequencyModifier = ((Mathf.PerlinNoise(Time.time * 0.05f * freqRand, t * freqRand2))/2) * 2.5f * (1/(amplitudeModifier));
       return Mathf.Sin(frequency * t * frequencyModifier + Time.time * speed) * amplitude * t * (1 - t) * amplitudeModifier;
        //return Mathf.Sin(frequency * t + Time.time * speed) * amplitude * (1 - t); //decrease amplitude from left to right, max to min
    }
    public float GetAmplitude()
    {
        return amplitude;
    }
    public void setAmplitude(float a)
    {
        amplitude = a;
    }
    public void setFrequency(float f)
    {
        frequency = f;
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
