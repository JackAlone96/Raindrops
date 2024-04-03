using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    private Coroutine fadeInCoroutine;
    private Coroutine fadeOutCoroutine;

    [SerializeField] private CanvasGroup gameoverCanvasGroup;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;

    private void OnEnable()
    {
        gameoverCanvasGroup.alpha = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartFadeInAnimation(int bestScore, int score)
    {
        bestScoreText.text = "Best Score: " + bestScore.ToString();
        scoreText.text = "Score: " + score.ToString();
        fadeInCoroutine = StartCoroutine(FadeInCO());
    }

    public void StartFadeOutAnimation()
    {
        fadeOutCoroutine = StartCoroutine(FadeOutCO());
    }

    private IEnumerator FadeInCO()
    {
        while (gameoverCanvasGroup.alpha < 1)
        {
            gameoverCanvasGroup.alpha += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FadeOutCO()
    {
        while (gameoverCanvasGroup.alpha > 0)
        {
            gameoverCanvasGroup.alpha -= Time.deltaTime;
            yield return null;
        }
    }
}
