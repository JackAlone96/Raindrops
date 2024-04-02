using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TMP_InputField inputText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    private bool isGamePaused = false;
    int totalScore = 0;

    private void OnEnable()
    {
        EventManager<bool>.Instance.StartListening("onGamePaused", PauseGame);
        EventManager<int>.Instance.StartListening("onTearPopped", ManageScore);
    }

    private void OnDisable()
    {
        EventManager<bool>.Instance.StopListening("onGamePaused", PauseGame);
        EventManager<int>.Instance.StopListening("onTearPopped", ManageScore);
    }

    // Start is called before the first frame update
    void Start()
    {
        inputText.Select();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.currentState == GameManager.GameState.PLAYING)
        {
            timeText.text = "Time: " + GameManager.Instance.gameTimeSpan.ToString(@"mm\:ss");
        }
    }

    private void PauseGame(bool pause)
    {
        isGamePaused = pause;
        if (isGamePaused)
        {
            inputText.text = "";
            inputText.DeactivateInputField();
        }
        else
        {
            inputText.ActivateInputField();
            inputText.Select();
        }
    }

    public void SendAnswer()
    {
        string answer = inputText.text;
        inputText.text = "";
        EventManager<string>.Instance.TriggerEvent("onInput", answer);
        inputText.ActivateInputField();
        inputText.Select();
    }

    private void ManageScore(int score)
    {
        totalScore += score;
        scoreText.text = "Score: " + totalScore.ToString();
    }
}
