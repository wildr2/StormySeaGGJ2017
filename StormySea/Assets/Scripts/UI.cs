using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private Boat boat;
    public Text score_text, restart_text;
    
    private void Awake()
    {
        boat = FindObjectOfType<Boat>();
        boat.on_boat_die += OnBoatDie;
    }
    private void Update()
    {
        score_text.text = "Score : " + (int)boat.Score;
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
