using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static int lives;
    public Text livesText;
    public static float hungerValue;
    public Image hungerBar;
    public static int score;
    public Text scoreText;


    // Start is called before the first frame update
    void Start()
    {
        lives = 30;
        livesText.text = "X 30";

        hungerValue = 0;
        hungerBar.fillAmount = 0;

        score = 0;
        scoreText.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        livesText.text = "X " + lives;
        hungerBar.fillAmount = hungerValue;
        scoreText.text = score.ToString();

        if (hungerValue >= 1.0f)
        {
            hungerValue = 0;
            lives++;
        }
    }   
}
