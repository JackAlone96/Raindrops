using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    private float elapsedTime;
    public float duration = 1f;
    public float animationSpeed = 2;
    private Coroutine popInCoroutine;
    private Coroutine popOutCoroutine;

    private void OnEnable()
    {
        EventManagerOneParam<bool>.Instance.StartListening("onGamePaused", StartPopOutAnimation);
    }

    private void OnDisable()
    {
        EventManagerOneParam<bool>.Instance.StopListening("onGamePaused", StartPopOutAnimation);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator PopInCO()
    {
        elapsedTime = 0;
        Vector3 startScale = transform.localScale;
        if (popOutCoroutine != null)
        {
            StopCoroutine(popOutCoroutine);
            popOutCoroutine = null;
        }

        while (transform.localScale.x < 1)
        {
            elapsedTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, Vector3.one, (elapsedTime / duration) * animationSpeed);
            yield return null;
        }
        elapsedTime = 0;
        popInCoroutine = null;
        transform.localScale = Vector3.one;
    }

    private IEnumerator PopOutCO()
    {
        elapsedTime = 0;
        Vector3 startScale = transform.localScale;
        if (popInCoroutine != null)
        {
            StopCoroutine(popInCoroutine);
            popInCoroutine = null;
        }

        while (transform.localScale.x > 0)
        {
            elapsedTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, (elapsedTime / duration) * animationSpeed);
            yield return null;
        }
        elapsedTime = 0;
        popOutCoroutine = null;
        transform.localScale = Vector3.zero;
        EventManagerOneParam<bool>.Instance.TriggerEvent("onPausePanel", false);
        gameObject.SetActive(false);
    }

    public void StartPopInAnimation()
    {
        EventManagerOneParam<bool>.Instance.TriggerEvent("onPausePanel", true);
        popInCoroutine = StartCoroutine(PopInCO());
    }

    public void StartPopOutAnimation(bool pause)
    {
        if (!pause) popOutCoroutine = StartCoroutine(PopOutCO());
    }
}
