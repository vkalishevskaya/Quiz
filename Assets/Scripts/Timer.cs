using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public bool isAnsweringQuestion = false;
    float timerValue;
    public float fillFraction;
    public bool loadNextQuestion;
    [SerializeField] float timeToCompleteQuestion = 30f;
    [SerializeField] float timeToShowCorrectAnswer = 5f;

    void Update()
    {
        UpdateTimer();

    }

    public void CancelTimer()
    {
        timerValue = 0;
    }

    void UpdateTimer()
    {

        timerValue -= Time.deltaTime;

        if (isAnsweringQuestion)
        {
            if (timerValue > 0)
            {
                fillFraction = timerValue / timeToCompleteQuestion;

            }

            else
            {
                isAnsweringQuestion = false;
                timerValue = timeToShowCorrectAnswer;
            }

        }

        else
        {
            if (timerValue > 0)
            {
                fillFraction = timerValue / timeToShowCorrectAnswer;
            }

            else
            {
                isAnsweringQuestion = true;
                timerValue = timeToCompleteQuestion;
                loadNextQuestion = true;
            }
        }
    }
}
