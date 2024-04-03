using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TMP_InputField inputText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject mainmenuPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameoverPanel;
    [SerializeField] private PointsText pointsText;
    [SerializeField] private Image fadeoutPanel;
    private bool isGamePaused = false;
    int totalScore = 0;

    private void OnEnable()
    {
        EventManager<bool>.Instance.StartListening("onGamePaused", PauseGame);
        EventManager<bool>.Instance.StartListening("onGameStarted", SelectInputField);
        EventManager<int>.Instance.StartListening("onGameover", Gameover);
        EventManager<int>.Instance.StartListening("onTearPopped", ManageScore);
        EventManager<ScriptableGameDifficulty>.Instance.StartListening("onDifficultyChanged", DifficultyText);
    }

    private void OnDisable()
    {
        EventManager<bool>.Instance.StopListening("onGamePaused", PauseGame);
        EventManager<bool>.Instance.StopListening("onGameStarted", SelectInputField);
        EventManager<int>.Instance.StopListening("onGameover", Gameover);
        EventManager<int>.Instance.StopListening("onTearPopped", ManageScore);
        EventManager<ScriptableGameDifficulty>.Instance.StopListening("onDifficultyChanged", DifficultyText);
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
            pausePanel.SetActive(true);
            pausePanel.GetComponent<PauseUI>().StartPopInAnimation();
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

    private void DifficultyText(ScriptableGameDifficulty scriptableGameDifficulty)
    {
        if (scriptableGameDifficulty.levelNumber <= 1) return;
        Instantiate(pointsText, scoreText.transform.position + new Vector3(-20, 40), Quaternion.identity, canvas.transform).Init("LEVEL UP!");
    }

    private void Gameover(int bestScore)
    {
        gameoverPanel.SetActive(true);
        gameoverPanel.GetComponent<GameOverUI>().StartFadeInAnimation(bestScore, totalScore);
    }

    private void SelectInputField(bool select)
    {
        inputText.Select();
    }

    public void RestartGame()
    {
        fadeoutPanel.color = new Color(1, 1, 1, 0);
        fadeoutPanel.gameObject.SetActive(true);
        StartCoroutine(FadeOutCO());
    }

    public void StartFadeInMainMenu()
    {
        StartCoroutine(FadeInCO());
    }

    private IEnumerator FadeOutCO()
    {
        while (fadeoutPanel.color.a < 1)
        {
            float newAlpha = fadeoutPanel.color.a + Time.deltaTime;
            fadeoutPanel.color = new Color(0, 0, 0, newAlpha);
            yield return null;
        }
        EventManager<bool>.Instance.TriggerEvent("onFadeOut", true);
    }

    private IEnumerator FadeInCO()
    {
        while (fadeoutPanel.color.a > 0)
        {
            float newAlpha = fadeoutPanel.color.a - Time.deltaTime;
            fadeoutPanel.color = new Color(0, 0, 0, newAlpha);
            yield return null;
        }
        fadeoutPanel.gameObject.SetActive(false);
    }
}
