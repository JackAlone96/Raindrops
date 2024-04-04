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
    public int result;

    private void OnEnable()
    {
        EventManagerOneParam<bool>.Instance.StartListening("onPausePanel", HideText);
    }

    private void OnDisable()
    {
        EventManagerOneParam<bool>.Instance.StopListening("onPausePanel", HideText);
    }

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

    public void Init(int FirstNumber, int SecondNumber, string Operand, float Speed, int Score, int Result)
    {
        firstNumber.text = FirstNumber.ToString();
        secondNumber.text = SecondNumber.ToString();
        if ((Operand == "&" || Operand == "|") && (firstNumber.text.Length < 3 || secondNumber.text.Length < 3))
        {
            if (firstNumber.text.Length < 3)
            {
                string stringToAdd = "";
                for (int i = 0; i < 3 - firstNumber.text.Length; i++)
                {
                    stringToAdd += "0";
                }
                firstNumber.text = stringToAdd + firstNumber.text;
            }

            if (secondNumber.text.Length < 3)
            {
                string stringToAdd = "";
                for (int i = 0; i < 3 -secondNumber.text.Length; i++)
                {
                    stringToAdd += "0";
                }
                secondNumber.text = stringToAdd + secondNumber.text;
            }
        }
        operand.text = Operand;
        speed = Speed;
        score = Score;
        result = Result;
    }

    virtual public void Pop()
    {
        EventManagerOneParam<int>.Instance.TriggerEvent("onTearPopped", score);
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
        EventManagerOneParam<Tear>.Instance.TriggerEvent("onTearLanded", this);
        TearFactory.Instance.ReturnObject(gameObject);
    }

    private void HideText(bool hide)
    {
        float alpha = hide == true ? 0 : 1;

        firstNumber.alpha = alpha;
        secondNumber.alpha = alpha;
        operand.alpha = alpha;
    }
}
