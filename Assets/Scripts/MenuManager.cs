using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Text[] distanceText;
    [SerializeField] private Text[] ScoreText;

    // Update is called once per frame
    void Update()
    {

    }

    public void Play()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadRecords()
    {
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

            for (int i = 0; i < 5; i++)
            {
                distanceText[i].text = GameRecord.distanceRanking[i].ToString("0.0") + " m";
            }
        }
        else
        {
            Debug.Log("File doesn't exist!");
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

            for (int i = 0; i < 5; i++)
            {
                ScoreText[i].text = GameRecord.scoreRanking[i].ToString();
            }
        }
        else
        {
            Debug.Log("File doesn't exist!");
        }
    }
}
