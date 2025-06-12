using UnityEngine;
using TMPro;
using UnityEngine.UI;
//using System;
using System.Collections.Generic;


public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    QuestionSO currentQuestion;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    [SerializeField] TextMeshProUGUI questionText;

    [Header("Answers")]
    [SerializeField] GameObject[] answersButtons;
    int correctAnswerIndex;
    bool hasAnsweredEarly = true;

    [Header("Images")]
    [SerializeField] Image questionImage;
    [SerializeField] Image[] answerImage;

    [Header("Button Colors")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;
    [SerializeField] Sprite wrongAnswerSprite;


    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("Slider")]
    [SerializeField] Slider progressBar;
    public bool isComplete;

    [Header("Audio")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioClip correctAnswerAudio;
    [SerializeField] AudioClip wrongAnswerAudio;
    [SerializeField] AudioClip timeIsUpAudio;


    void Awake()
    {
        timer = FindAnyObjectByType<Timer>();
        scoreKeeper = FindAnyObjectByType<ScoreKeeper>();
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    void Update()
    {
        timerImage.fillAmount = timer.fillFraction;
        if (timer.loadNextQuestion)
        {
            if (progressBar.value == progressBar.maxValue)
            {
                isComplete = true;
                return;
            }

            hasAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }



        else if (!hasAnsweredEarly && !timer.isAnsweringQuestion)
        {
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }

    void SetButtonState(bool state)
    {
        for (int i = 0; i < answersButtons.Length; i++)
        {
            Button button = answersButtons[i].GetComponent<Button>();
            button.interactable = state;
        }

    }
    public void OnAnswerSelected(int index)
    {
        hasAnsweredEarly = true;
        DisplayAnswer(index);
        SetButtonState(false);
        timer.CancelTimer();
    }

    void DisplayAnswer(int index)
    {
        Image buttonImage;

        if (index == currentQuestion.GetCorrectAnswerIndex())
        {
            questionText.text = "Зетс райт";
            PauseMusicAndPlay(correctAnswerAudio);
            buttonImage = answersButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            scoreKeeper.IncrementCorrectAnswers();
        }
        else if (index == -1)
        {
            correctAnswerIndex = currentQuestion.GetCorrectAnswerIndex();
            string correctAnswer = currentQuestion.GetAnswer(correctAnswerIndex).answerText;
            questionText.text = "Правильный ответ: \n" + correctAnswer;
            PauseMusicAndPlay(timeIsUpAudio);
            buttonImage = answersButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            hasAnsweredEarly = true;
        }
        else
        {
            correctAnswerIndex = currentQuestion.GetCorrectAnswerIndex();
            string correctAnswer = currentQuestion.GetAnswer(correctAnswerIndex).answerText;
            questionText.text = "Правильный ответ: \n" + correctAnswer;
            PauseMusicAndPlay(wrongAnswerAudio);
            buttonImage = answersButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            buttonImage = answersButtons[index].GetComponent<Image>();
            buttonImage.sprite = wrongAnswerSprite;
        }
        scoreKeeper.IncrementSeenQuestions();
        scoreText.text = "Score: " + scoreKeeper.CalculateScore() + "%";
    }

    void GetNextQuestion()
    {
        if (questions.Count > 0)
        {

            SetButtonState(true);
            SetDefaultButtinSprite();
            GetRandomQuestion();
            DisplayQuestion();
            progressBar.value++;
        }
    }

    private void GetRandomQuestion()
    {
        int index = Random.Range(0, questions.Count);
        currentQuestion = questions[index];
        if (questions.Contains(currentQuestion))
        {
            questions.Remove(currentQuestion);
        }

    }

    void DisplayQuestion()
    {
        questionText.text = currentQuestion.GetQuestionText();

        if (currentQuestion.GetQuestionImage() != null)
        {
            questionImage.sprite = currentQuestion.GetQuestionImage();
            questionImage.gameObject.SetActive(true);
        }
        else
        {
            questionImage.gameObject.SetActive(false);
        }


        for (int i = 0; i < answersButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answersButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestion.GetAnswer(i).answerText;

            if (currentQuestion.GetAnswer(i).answerPicture != null)
            {
                answerImage[i].sprite = currentQuestion.GetAnswer(i).answerPicture;
                answerImage[i].gameObject.SetActive(true);
            }
            else
            {
                answerImage[i].gameObject.SetActive(false);
            }
        }

    }

    void SetDefaultButtinSprite()
    {
        for (int i = 0; i < answersButtons.Length; i++)
        {
            Image buttonImage = answersButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }
    void PauseMusicAndPlay(AudioClip clip)
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Pause();
        }

        GetComponent<AudioSource>().PlayOneShot(clip);

        if (musicSource != null && clip != null)
        {
            StartCoroutine(ResumeMusicAfterDelay(clip.length));
        }
    }
    System.Collections.IEnumerator ResumeMusicAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (musicSource != null)
        {
            musicSource.UnPause();
        }
    }        
}