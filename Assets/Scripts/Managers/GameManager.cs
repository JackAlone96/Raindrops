using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public enum GameState { IDLE, MAIN_MENU, PLAYING, PAUSE, GAMEOVER };

    public GameState currentState { get; private set; }

    [SerializeField] private GameObject sea;
    [SerializeField] private float seaIncrement = 0.5f;

    private ScriptableGameDifficulty[] gameDifficulties;
    private int difficultyIndex = 0;

    private bool isGamePaused = false;
    private int totalScore = 0;
    private int partialScore = 0;
    private int bestScore = 0;
    private float gameTime = 0;
    public TimeSpan gameTimeSpan;

    private Coroutine seaCoroutine;

    private void OnEnable()
    {
        EventManager<bool>.Instance.StartListening("onGamePaused", PauseGame);
        EventManager<int>.Instance.StartListening("onTearPopped", ManageDifficulty);
        EventManager<Tear>.Instance.StartListening("onTearLanded", ManageSea);
        EventManager<bool>.Instance.StartListening("onFadeOut", RestarGame);
        EventManager<bool>.Instance.StartListening("onGameStarted", StartGame);
    }

    private void OnDisable()
    {
        EventManager<bool>.Instance.StopListening("onGamePaused", PauseGame);
        EventManager<int>.Instance.StopListening("onTearPopped", ManageDifficulty);
        EventManager<Tear>.Instance.StopListening("onTearLanded", ManageSea);
        EventManager<bool>.Instance.StopListening("onFadeOut", RestarGame);
        EventManager<bool>.Instance.StopListening("onGameStarted", StartGame);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get all the game difficulties and initialize the first one
        gameDifficulties = Resources.LoadAll<ScriptableGameDifficulty>("GameDifficulties");
        EventManager<ScriptableGameDifficulty>.Instance.TriggerEvent("onDifficultyChanged", gameDifficulties[difficultyIndex]);

        // Set the current state
        currentState = GameState.MAIN_MENU;

        // Start animation
        UIManager.Instance.StartFadeInMainMenu();

        // Load best score
        bestScore = DataManager.Instance.LoadBestScore();
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case GameState.IDLE:

                break;
            case GameState.MAIN_MENU:

                break;
            case GameState.PLAYING:
                gameTime += Time.deltaTime;
                gameTimeSpan = TimeSpan.FromSeconds(gameTime);
                break;
            case GameState.PAUSE:

                break;
            case GameState.GAMEOVER:

                break;
        }
    }

    private void PauseGame(bool pause)
    {
        isGamePaused = pause;
        if (pause)
        {
            currentState = GameState.PAUSE;
        }
        else
        {
            currentState = GameState.PLAYING;
        }
    }

    private void ManageDifficulty(int score)
    {
        totalScore += score;
        partialScore += score;
        if (difficultyIndex < gameDifficulties.Length - 1)
        {
            if (partialScore >= gameDifficulties[difficultyIndex].scoreToClearLevel)
            {
                Debug.Log("LEVEL UP at" + totalScore + " points with a " + partialScore + " base");
                difficultyIndex++;
                partialScore = 0;
                EventManager<ScriptableGameDifficulty>.Instance.TriggerEvent("onDifficultyChanged", gameDifficulties[difficultyIndex]);
            }
        }
    }

    private void ManageSea(Tear tear)
    {
        Vector2 newPos = sea.transform.position + new Vector3(0, seaIncrement, 0);

        if ((newPos.y + sea.GetComponent<SpriteRenderer>().bounds.extents.y) >= (Camera.main.transform.position.y + Camera.main.orthographicSize))
        {
            currentState = GameState.GAMEOVER;
            if (totalScore > bestScore)
            {
                DataManager.Instance.SaveBestScore(totalScore);
            }
            EventManager<int>.Instance.TriggerEvent("onGameover", bestScore);



            return;
        }

        if (seaCoroutine == null)
        {
            seaCoroutine = StartCoroutine(SeaLevelCoroutine(newPos, 0.5f));
        }
    }

    private IEnumerator SeaLevelCoroutine(Vector2 newPos, float duration)
    {
        float time = 0;
        Vector2 startPos = sea.transform.position;
        while (time < duration)
        {
            sea.transform.position = Vector2.Lerp(startPos, newPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = newPos;
        seaCoroutine = null;
    }

    public void RestarGame(bool restart)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void StartGame(bool start)
    {
        currentState = GameState.PLAYING;
    }
}
