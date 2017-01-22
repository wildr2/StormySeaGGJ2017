using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Boat : MonoBehaviour 
{
    public const float ScrollSpeed = 1;

    private Ocean ocean;

    private Rigidbody2D rb;
    public SpriteRenderer sprite_r;
    public ParticleSystem fire;
    public SpriteRenderer lightning;

    private float speed = 3f;
    private float slope_speed = 50f;
    private float velocity;

    private float x_boundary = 9.5;

    private float shock_build = 0;
    private bool alive = true;

    public bool BuildShock(float amount)
    {
        bool shocked = false;

        shock_build += amount;
        if (shock_build >= 1)
        {
            OnShock();
            shocked = true;
        }

        UpdateShockIndicator();

        return shocked;
    }


    private void Awake()
    {
        ocean = FindObjectOfType<Ocean>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Rock>() != null)
            OnDie();
    }
    private void Update()
    {
        // shock diminish
        shock_build = Mathf.Max(0, shock_build - Time.deltaTime);
        UpdateShockIndicator();
    }
    private void FixedUpdate()
    {
        UpdateMovement();
    }
    private void UpdateMovement()
    {
        Vector2 pos = rb.position;
        Vector2 ocean_pos = new Vector2(pos.x, ocean.GetHeight(pos.x));
        Vector2 ocean_pos_left = new Vector2(pos.x - 0.01f, ocean.GetHeight(pos.x - 0.01f));
        Vector2 tangent = (ocean_pos - ocean_pos_left).normalized;


        if (alive)
        {
            // Acceleration
            float acceleration = 0;

            // input
            float input_x = Input.GetAxis("Horizontal");
            acceleration += input_x * speed;

            // slope
            float slope = tangent.y / tangent.x;
            float slope_factor = slope >= 0 ? 1f / (slope * slope_speed + 1) : (-slope * slope_speed + 1);
            acceleration -= slope * 1f;

            // velocity, position
            velocity += acceleration * Time.deltaTime;
            velocity /= 1 + (1.5f * Time.deltaTime);

            Vector2 newpos = pos + Vector2.right * velocity * Time.deltaTime;
            newpos.y = ocean_pos.y; // wave attachment
            newpos.x = Mathf.Clamp(newpos.x, -x_boundary, x_boundary);
            rb.MovePosition(newpos);

            // Rotation
            //if (pos.y <= ocean_pos.y)
            //{
            float target_r = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;
            rb.MoveRotation(Mathf.Lerp(rb.rotation, target_r, Time.deltaTime * 4f));
            //}
        }
        else
        {
            rb.gravityScale = pos.y < ocean_pos.y ? 0.05f : 1;
        }
        
    }
    private void LateUpdate()
    {

    }

    
    private void OnDie()
    {
        alive = false;
        StartCoroutine(RestartAfterDeath());
    }
    private IEnumerator RestartAfterDeath()
    {
        yield return new WaitForSeconds(1);
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        SceneManager.LoadScene(0);
    }

    private void OnShock()
    {
        shock_build = 0;
        StartCoroutine(FlashLightning());
        OnDie();
    }
    private void UpdateShockIndicator()
    {
        //if (shock_build == 0) fire.enableEmission = false;
        //else fire.enableEmission = true;
        fire.startLifetime = shock_build;
        sprite_r.color = Color.Lerp(Color.white, Color.yellow, shock_build);
    }

    private IEnumerator FlashLightning()
    {
        lightning.gameObject.SetActive(true);
        for (float t = 0; t < 0.1f; t += Time.deltaTime)
        {
            //lightning.transform.rotation = Quaternion.Euler(0, 0, 0);

            yield return null;
        }
        lightning.gameObject.SetActive(false);
    }

}
