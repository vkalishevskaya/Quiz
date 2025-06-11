using System;
using UnityEngine;


[CreateAssetMenu(fileName = "New Question", menuName = "Quiz Question")]
public class QuestionSO : ScriptableObject 
{
    [TextArea(2,6), SerializeField] string question = "Enter question here";
    [SerializeField] Sprite questionPicture;
    [SerializeField] Answer[] answers = new Answer[4];
    [SerializeField] int correctAnswerIndex;

    public string GetQuestionText()
    {
        return question;
    }

    public Sprite GetQuestionImage()
    {
        return questionPicture;
    }

    public Answer GetAnswer(int index)
    {
        return answers[index];
    }

    public int GetCorrectAnswerIndex()
    {
        return correctAnswerIndex;
    }


}

[Serializable] public class Answer
{
    public string answerText;
    public Sprite answerPicture;
}
