using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveBestScore(int score)
    {
        if (score < 0) return;

        PlayerPrefs.SetInt("bestScore", score);
        PlayerPrefs.Save();
    }

    public int LoadBestScore()
    {
        return PlayerPrefs.GetInt("bestScore", 0);
    }
}
