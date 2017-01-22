using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Boat : MonoBehaviour 
{
    private Ocean ocean;
    private ScrollManager scroller;

    private Rigidbody2D rb;
    public SpriteRenderer alive_sr, dead_sr, shock_sr;
    public ParticleSystem fire;
    public SpriteRenderer lightning, glow;

    private float speed = 6f;
    private float slope_slide = 2f;
    private float velocity;

    private float x_boundary = 8f;

    private bool alive = true;
    private float input_seconds = 0;
    public float Score { get; private set; }

    private float shock_build_rate = 0.75f;
    private float shock_fall_rate = 0.75f;
    private float shock_build = 0;
    private Cloud shocking_cloud = null;

    public System.Action<Boat> on_boat_die;


    private void Awake()
    {
        ocean = FindObjectOfType<Ocean>();
        rb = GetComponent<Rigidbody2D>();
        scroller = FindObjectOfType<ScrollManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (alive)
        {
            if (collision.collider.GetComponent<Rock>() != null)
                OnDie();
        }   
    }
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (!alive) return;

        Cloud cloud = collider.GetComponentInParent<Cloud>();
        if (cloud != null && cloud.IsDangerous())
        {
            shocking_cloud = cloud;
        }
    }
    private void Update()
    {
        if (alive)
        {
            // shock diminish
            if (shocking_cloud != null)
            {
                shock_build += Time.deltaTime * shock_build_rate;
                if (shock_build >= 1)
                {
                    OnShock(shocking_cloud);
                }
            }
            else shock_build = Mathf.Max(0, shock_build - Time.deltaTime * shock_fall_rate);
            shocking_cloud = null;

            UpdateShockIndicator();
        }
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

            input_seconds += input_x != 0 ? Time.deltaTime : 0;
            Score = (int)((scroller.DistanceTravelled + transform.position.x - input_seconds) * 10f);

            // slope
            float slope = tangent.y / tangent.x;
            acceleration -= slope * slope_slide;

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
            if (pos.y > ocean_pos.y)
            {
                rb.drag = 0;
                rb.angularDrag = 0.1f;
            }
            else
            {
                rb.drag = 10;
                rb.angularDrag = 0.5f;
            }
        }
        
    }
    private void LateUpdate()
    {

    }

    
    private void OnDie()
    {
        alive = false;
        StartCoroutine(RestartAfterDeath());
        StartCoroutine(FadeGlow());

        scroller.Pause();

        if (on_boat_die != null) on_boat_die(this);
    }
    private IEnumerator RestartAfterDeath()
    {
        //yield return new WaitForSeconds(1);
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        SceneManager.LoadScene(0);
    }
    private IEnumerator FadeGlow()
    {
        Color c0 = glow.color;
        for (float t = 0; t < 1; t += Time.deltaTime / 5f)
        {
            glow.color = Color.Lerp(c0, Color.clear, t);
            alive_sr.color = Color.Lerp(Color.white, Color.clear, t);
            yield return null;
        }
    }

    private void OnShock(Cloud shocker)
    {
        shock_build = 1;
        fire.enableEmission = false;

        StartCoroutine(FlashLightning(shocker));

        rb.AddTorque((Random.value - 0.5f) * 2f, ForceMode2D.Impulse);

        OnDie();
    }
    private void UpdateShockIndicator()
    {
        if (shock_build == 0) fire.enableEmission = false;
        else fire.enableEmission = true;

        Color c = fire.startColor;
        c.a = shock_build;
        fire.startColor = c;

        shock_sr.color = Color.Lerp(Color.clear, Color.white, Mathf.Pow(shock_build, 4));
        shock_sr.enabled = shock_build > 0;
    }

    private IEnumerator FlashLightning(Cloud shocker)
    {
        lightning.gameObject.SetActive(true);
        for (float t = 0; t < 0.1f; t += Time.deltaTime)
        {
            Vector2 p0 = transform.position;
            Vector2 p1 = shocker.transform.position;
            Vector2 dir = (p1 - p0).normalized;

            float l_height = lightning.sprite.bounds.size.y * lightning.transform.localScale.y;

            lightning.transform.position = p0 + dir * (l_height * 0.5f - 0.1f);
            lightning.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);

            yield return null;
        }
        lightning.gameObject.SetActive(false);

        shock_build = 0;
        UpdateShockIndicator();
    }

}
