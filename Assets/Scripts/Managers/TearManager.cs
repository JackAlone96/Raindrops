using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Timers;
using UnityEngine;
using System;

public class TearManager : Singleton<TearManager>
{
    enum OperationTypes { Addition = 0, Multiplication = 1, Substraction = 2, Division = 3, And = 4, Or = 5};

    public float spawnRate;
    [Range(1, 100)]
    public int goldenTearSpawnProbability;

    private float gameTime = 0f;
    private Vector3 SpawnArea;

    public Dictionary<float, List<Tear>> tearDictionary = new Dictionary<float, List<Tear>>();
    private List<Tear> tearsInGame = new List<Tear>();

    private ScriptableGameDifficulty currentDifficulty;

    private void OnEnable()
    {
        EventManager<string>.Instance.StartListening("onInput", CheckAnswer);
        EventManager<ScriptableGameDifficulty>.Instance.StartListening("onDifficultyChanged", ChangeDifficulty);
        EventManager<Tear>.Instance.StartListening("onTearLanded", TearLanded);
    }

    private void OnDisable()
    {
        EventManager<string>.Instance.StopListening("onInput", CheckAnswer);
        EventManager<ScriptableGameDifficulty>.Instance.StopListening("onDifficultyChanged", ChangeDifficulty);
        EventManager<Tear>.Instance.StopListening("onTearLanded", TearLanded);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameTime = spawnRate;
        SpawnArea = CalculateSpawnArea();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.currentState == GameManager.GameState.PLAYING)
        {
            if (gameTime <= 0)
            {
                gameTime = spawnRate;
                InitializeTear(SpawnTear());
            }
            else
            {
                gameTime -= Time.deltaTime;
            }
        }
    }

    public void CheckAnswer(string answer)
    {
        float answerFloat;
        if (float.TryParse(answer, out answerFloat))
        {
            List<Tear> tearList;
            if (tearDictionary.TryGetValue(answerFloat, out tearList))
            {
                foreach (Tear tear in tearList)
                {
                    tearsInGame.Remove(tear);
                    tear.Pop();
                }
                tearDictionary.Remove(answerFloat);
            }
        }
    }

    private void ChangeDifficulty(ScriptableGameDifficulty newDifficulty)
    {
        currentDifficulty = newDifficulty;
        spawnRate = currentDifficulty.spawnInterval;
    }

    private Tear SpawnTear()
    {
        float spawnXPos = UnityEngine.Random.Range(SpawnArea.x, SpawnArea.y);
        Vector3 spawnPos = new Vector3(spawnXPos, SpawnArea.z, 0f);
        Tear tear;

        if (UnityEngine.Random.Range(1,101) <= goldenTearSpawnProbability)
        {
            tear = TearFactory.Instance.CreateObject("GoldenTearPrefab", spawnPos, Quaternion.identity).GetComponent<Tear>();
            return tear;
        }
        
        tear = TearFactory.Instance.CreateObject("TearPrefab", spawnPos, Quaternion.identity).GetComponent<Tear>();

        return tear;
    }

    private void InitializeTear(Tear tear)
    {
        int firstNumber = UnityEngine.Random.Range(0, currentDifficulty.rangeMaxNumber);
        int secondNumber = UnityEngine.Random.Range(0, currentDifficulty.rangeMaxNumber);
        int result = 0;
        OperationTypes operation = (OperationTypes)UnityEngine.Random.Range(0, currentDifficulty.operationMaxNumber);
        string operationString = "+";

        // Calculate the result of the operation
        switch (operation)
        {
            case OperationTypes.Addition:
                result = firstNumber + secondNumber;
                operationString = "+";
                break;
            case OperationTypes.Substraction:
                firstNumber = UnityEngine.Random.Range(0, currentDifficulty.rangeMaxNumber);
                secondNumber = UnityEngine.Random.Range(0, firstNumber);
                result = firstNumber - secondNumber;
                operationString = "-";
                break;
            case OperationTypes.Multiplication:
                result = firstNumber * secondNumber;
                operationString = "x";
                break;
            case OperationTypes.Division:
                secondNumber = UnityEngine.Random.Range(1, currentDifficulty.rangeMaxNumber);
                firstNumber = UnityEngine.Random.Range(1, currentDifficulty.rangeMaxNumber) * secondNumber;
                result = firstNumber / secondNumber;
                operationString = "/";
                break;
            case OperationTypes.And:
                firstNumber = Helper.GenerateRandomBinaryNumber(3);
                secondNumber = Helper.GenerateRandomBinaryNumber(3);

                int firstNumberDecimal = Convert.ToInt32(firstNumber.ToString(), 2);
                int secondNumberDecimal = Convert.ToInt32(secondNumber.ToString(), 2);
                operationString = "&";
                result = int.Parse(Convert.ToString(firstNumberDecimal & secondNumberDecimal, 2));
                break;
            case OperationTypes.Or:
                firstNumber = Helper.GenerateRandomBinaryNumber(3);
                secondNumber = Helper.GenerateRandomBinaryNumber(3);

                firstNumberDecimal = Convert.ToInt32(firstNumber.ToString(), 2);
                secondNumberDecimal = Convert.ToInt32(secondNumber.ToString(), 2);
                operationString = "|";
                result = int.Parse(Convert.ToString(firstNumberDecimal | secondNumberDecimal, 2));
                break;
        }

        tear.Init(firstNumber, secondNumber, operationString, currentDifficulty.tearSpeed, currentDifficulty.tearPoints, result);
        tearsInGame.Add(tear);

        // Insert the tear in the dictionary checking if it was already present or not
        List<Tear> tearList = new List<Tear>();
        if (tearDictionary.ContainsKey(result))
        {
            tearDictionary.TryGetValue(result, out tearList);
            tearList.Add(tear);
        }
        else
        {
            tearList.Add(tear);
            tearDictionary.Add(result, tearList);
        }
    }

    private Vector3 CalculateSpawnArea()
    {
        float TearHalfWidth = TearFactory.Instance.prefabs[0].GetComponent<SpriteRenderer>().bounds.extents.x;
        float TearHalfHeight = TearFactory.Instance.prefabs[0].GetComponent<SpriteRenderer>().bounds.extents.y;
        Vector3 SpawnArea = new Vector2();
        SpawnArea.x = Camera.main.transform.position.x - Camera.main.orthographicSize * Screen.width / Screen.height + TearHalfWidth;
        SpawnArea.y = Camera.main.transform.position.x + Camera.main.orthographicSize * Screen.width / Screen.height - TearHalfWidth;
        SpawnArea.z = Camera.main.transform.position.y + Camera.main.orthographicSize + TearHalfHeight;

        return SpawnArea;
    }

    public void GoldenTearPopped()
    {
        for (int i = tearsInGame.Count - 1; i >= 0; i--)
        {
            Tear tear = tearsInGame[i];
            tearsInGame.Remove(tear);
            tear.Pop();
        }
        tearDictionary.Clear();
    }

    private void TearLanded(Tear tear)
    {
        tearsInGame.Remove(tear);
        List<Tear> tears;
        tearDictionary.TryGetValue(tear.result, out tears);
        tears.Remove(tear);
    }
}
