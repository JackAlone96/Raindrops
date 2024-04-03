using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Difficulty", menuName = "Create Game Difficulty")]
public class ScriptableGameDifficulty : ScriptableObject
{
    public int rangeMaxNumber;
    public int operationMaxNumber;
    public float tearSpeed;
    public int tearPoints;
    public int scoreToClearLevel;
    public int levelNumber;
    public float spawnInterval;
}
