using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private Boat boat;
    public Text score_text;
    
    private void Awake()
    {
        boat = FindObjectOfType<Boat>();
    }
    private void Update()
    {
        score_text.text = "Score : " + (int)boat.Score;
    }

}
