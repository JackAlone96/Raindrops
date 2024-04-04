using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputController : MonoBehaviour
{
    private bool isGamePaused = false;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && (GameManager.Instance.currentState == GameManager.GameState.PLAYING || GameManager.Instance.currentState == GameManager.GameState.PAUSE))
        {
            isGamePaused = isGamePaused == false ? true : false;
            EventManagerOneParam<bool>.Instance.TriggerEvent("onGamePaused", isGamePaused);
        }
    }
}
