using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tear : MonoBehaviour
{
    [SerializeField] private TextMeshPro firstNumber;
    [SerializeField] private TextMeshPro secondNumber;
    [SerializeField] private TextMeshPro operand;
    [SerializeField] private PointsText pointsText;
    private float speed;
    public int score;
    private float TearHalfHeight;

    // Start is called before the first frame update
    void Start()
    {
        TearHalfHeight = GetComponent<SpriteRenderer>().bounds.extents.y;    
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.currentState == GameManager.GameState.PLAYING)
        {
            Vector2 pos = transform.position;
            pos += Vector2.down * speed * Time.deltaTime;
            transform.position = pos;
            
            if (CheckCollision())
            {
                LoseLife();
            }
        }
    }

    public void Init(int FirstNumber, int SecondNumber, string Operand, float Speed, int Score)
    {
        firstNumber.text = FirstNumber.ToString();
        secondNumber.text = SecondNumber.ToString();
        operand.text = Operand;
        speed = Speed;
        score = Score;
    }

    virtual public void Pop()
    {
        EventManager<int>.Instance.TriggerEvent("onTearPopped", score);
        TearFactory.Instance.ReturnObject(gameObject);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Instantiate(pointsText, screenPos, pointsText.gameObject.transform.rotation, FindFirstObjectByType<Canvas>().transform).Init("+ " + score);
    }

    private bool CheckCollision()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, Vector2.down, TearHalfHeight);
        if (hit2D) return true;
        
        return false;
    }

    private void LoseLife()
    {
        EventManager<float>.Instance.TriggerEvent("onTearLanded", 0.5f);
        TearFactory.Instance.ReturnObject(gameObject);
    }
}
