using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Player status")]
    public static float foodValue;
    public Image foodBar;
    public static float drinkValue;
    public Image drinkBar;

    [Header("Game progress")]
    public static float score;
    public Text scoreText;
    public static float distance;
    public Text distanceText;

    [Header("Path Generation")]
    public GameObject[] pathArea;
    [SerializeField] private int pathIndex;
    [SerializeField] private int pathMax;

    [Header("Other Screens")]
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject gameoverScreen;
    [SerializeField] private Text finalDistance;
    [SerializeField] private Text finalScore;



    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        foodBar.fillAmount = foodValue;
        drinkBar.fillAmount = drinkValue;
        scoreText.text = score.ToString("0");
        distanceText.text = distance.ToString("0.0") + "m";

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void GenerateInitialPath()
    {
        Instantiate(pathArea[Random.Range(0, pathArea.Length)], new Vector3(0.62f, 0,  8.0f), Quaternion.identity);
        Instantiate(pathArea[Random.Range(0, pathArea.Length)], new Vector3(0.62f, 0, 16.0f), Quaternion.identity);
    }

    public void GenerateNewPath()
    {
        if (pathIndex < pathMax)
        {
            pathIndex += 1;
            Instantiate(pathArea[Random.Range(0, pathArea.Length)], new Vector3(0.62f, 0, 8.0f * pathIndex), Quaternion.identity);
            {
                if (pathIndex % 5 == 0)
                {
                    FindObjectOfType<PlayerController>().runSpeed += 0.15f;
                }
            }
        }
    }

    public void Reset()
    {
        foodValue = 0.5f;
        drinkValue = 0.5f;
        score = 0.0f;
        distance = 0.0f;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseScreen.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void Replay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void DisplayGameoverScreen()
    {
        finalDistance.text = "Distance: " + distance.ToString("0.0") + " m";
        finalScore.text = "Score: " + score.ToString();
        gameoverScreen.SetActive(true);
    }
}
