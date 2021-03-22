using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
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
    public static int score;
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

        if (File.Exists(Application.dataPath + "/Distance.txt")) 
        {
            FileStream fs = new FileStream(Application.dataPath + "/Distance.txt", FileMode.Open);

            StreamReader sr = new StreamReader(fs);
            for (int i = 0; i < 5; i++)
            {
                GameRecord.distanceRanking[i] = float.Parse(sr.ReadLine());
            }

            sr.Close();
            fs.Close();
        }

        if (File.Exists(Application.dataPath + "/Score.txt")) 
        {
            FileStream fs = new FileStream(Application.dataPath + "/Score.txt", FileMode.Open);

            StreamReader sr = new StreamReader(fs);
            for (int i = 0; i < 5; i++)
            {
                GameRecord.scoreRanking[i] = int.Parse(sr.ReadLine());
            }

            sr.Close();
            fs.Close();
        }
    }

    // Update is called once per frame
    void Update()
    {
        foodBar.fillAmount = foodValue;
        drinkBar.fillAmount = drinkValue;
        scoreText.text = score.ToString();
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
        score = 0;
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

        SaveDistanceRecords();
        SaveScoreRecords();

        gameoverScreen.SetActive(true);
    }

    private void SaveDistanceRecords()
    {
        for (int i = 0; i < 5; i++)
        {
            if (distance > GameRecord.distanceRanking[i])
            {
                for (int j = 4; j >= 0; j--)
                {
                    if (i == j)
                    {
                        GameRecord.distanceRanking[j] = distance;
                        break;
                    }
                    else
                    {
                        GameRecord.distanceRanking[j] = GameRecord.distanceRanking[j-1];
                    }
                }
                break;
            }
        }

        FileStream fs = new FileStream(Application.dataPath + "/Distance.txt", FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);
        for (int i = 0; i < 5; i++)
        {
            sw.WriteLine(GameRecord.distanceRanking[i]);
        }
        sw.Close();
        fs.Close();
    }
    private void SaveScoreRecords()
    {
        for (int i = 0; i < 5; i++)
        {
            if (score > GameRecord.scoreRanking[i])
            {
                for (int j = 4; j >= 0; j--)
                {
                    if (i == j)
                    {
                        GameRecord.scoreRanking[j] = score;
                        break;
                    }
                    else
                    {
                        GameRecord.scoreRanking[j] = GameRecord.scoreRanking[j-1];
                    }
                }
                break;
            }
        }

        FileStream fs = new FileStream(Application.dataPath + "/Score.txt", FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);
        for (int i = 0; i < 5; i++)
        {
            sw.WriteLine(GameRecord.scoreRanking[i]);
        }
        sw.Close();
        fs.Close();
    }
}
