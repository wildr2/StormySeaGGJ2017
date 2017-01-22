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
    public CanvasGroup lower_panel;
    public Text title;

    private void Awake()
    {
        boat = FindObjectOfType<Boat>();
        boat.on_boat_die += OnBoatDie;

        scroller = FindObjectOfType<ScrollManager>();
        StartCoroutine(TitleRoutine());
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
    private IEnumerator TitleRoutine()
    {
        title.color = Color.white;
        lower_panel.alpha = 0;

        yield return new WaitForSeconds(1f);

        for (float t = 0; t < 1; t += Time.deltaTime * 3f)
        {
            float tt = Mathf.Pow(t, 2);

            title.color = Color.Lerp(Color.white, Color.clear, tt);
            lower_panel.alpha = Mathf.Lerp(0, 1, tt);
            yield return null;
        }

        title.gameObject.SetActive(false);
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
