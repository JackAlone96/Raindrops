using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointsText : MonoBehaviour
{
    private TextMeshProUGUI textComponent;
    public float timeToDespawn = 0.75f;
    private float elapsedTime = 0;
    private Coroutine FadeCoroutine;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(string message)
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        textComponent.text = message;
        FadeCoroutine = StartCoroutine(FadeCO());
    }

    private IEnumerator FadeCO()
    {
        yield return new WaitForSeconds(0.5f);
        while (GameManager.Instance.currentState == GameManager.GameState.PLAYING && elapsedTime < timeToDespawn)
        {
            elapsedTime += Time.deltaTime;
            textComponent.alpha = Mathf.Lerp(1, 0, elapsedTime / timeToDespawn);
            yield return null;
        }
        if (textComponent.alpha <= 0) Destroy(gameObject);
    }
}
