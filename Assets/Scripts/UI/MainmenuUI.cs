using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainmenuUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup mainmenuCanvasGroup;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    
    // Start is called before the first frame update
    void Start()
    {
        bestScoreText.text = "Best Score: " + DataManager.Instance.LoadBestScore().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        StartCoroutine(StartGameCO());
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private IEnumerator StartGameCO()
    {
        while (mainmenuCanvasGroup.alpha > 0)
        {
            mainmenuCanvasGroup.alpha -= Time.deltaTime * 2;
            yield return null;
        }
        gameObject.SetActive(false);
        EventManager<bool>.Instance.TriggerEvent("onGameStarted", true);
    }
}
