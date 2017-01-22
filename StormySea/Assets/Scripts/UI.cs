using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private Boat boat;
    private ScrollManager scroller;
    public Text score_text, restart_text;
    public RectTransform progress_bar_filled;
    public RectTransform progress_ship;
    
    private void Awake()
    {
        boat = FindObjectOfType<Boat>();
        boat.on_boat_die += OnBoatDie;

        scroller = FindObjectOfType<ScrollManager>();
    }
    private void Update()
    {
        score_text.text = "Score : " + (int)boat.Score;
        UpdateProgress(boat.GetDistanceTravelled() / scroller.MapDistance);
    }

    private void UpdateProgress(float t)
    {
        float w = 1000;
        progress_bar_filled.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, t * w);
        //progress_ship.offsetMin = new Vector2(t * w, 0);
        progress_ship.anchoredPosition = new Vector2(t * w, 0);
        //progress_ship.localPosition = new Vector2(t * w, 0);
    }

    private void OnBoatDie(Boat boat)
    {
        StopAllCoroutines();
        StartCoroutine(BlinkRestartText());
    }
    private IEnumerator BlinkRestartText()
    {
        restart_text.gameObject.SetActive(false);
        yield return new WaitForSeconds(2);
        restart_text.gameObject.SetActive(true);

        while (true)
        {
            restart_text.enabled = !restart_text.enabled;
            yield return new WaitForSeconds(0.4f);
        }
    }

}
