using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static QuizController;

public class QuizController : MonoBehaviour
{
    // Start is called before the first frame update



    [System.Serializable]

    public class QuizData
    {
        public string quizId ;
        public string title ;
        public string description ;
        public List<Question> questions;
        public float score;
    }
    [System.Serializable]

    public class Question
    {
        public int questionId ;
        public string questionText ;
        public List<Option> options ;
        public int correctOptionId ;
    }
    [System.Serializable]

    public class Option
    {
        public string optionText ;
    }


    public GameObject canvasQuiz;
    public List<Button> buttons;
    public static QuizController instance;
    public float timer;
    [Header("Pre-Quiz")]
    public GameObject readyPanel;
    public GameObject quizPanel;
    public Button siapButton;
    public Button belumSiapButton;
    public TMP_Text judulTMP;
    public TMP_Text keteranganTMP;

    [Header("Quiz")]
    public TMP_Text TMPQuestion;
    public TMP_Text TMPQUizTimer;

    [Header("Score")]
    public GameObject timerHabisNotifikasi;
    public GameObject scorePanel;
    public TMP_Text TMPScore;
    public Button ulangiQuizButton;

    public Button backToMenuButton;
    public Button lanjutSubmateri;

    int choice = -1;
    public int correctAnswer;
    bool flagQuizDone;
    private void Awake()
    {
        instance = this;
        buttons.ForEach(x => x.gameObject.SetActive(false));

        siapButton.onClick.AddListener(Siap);
        belumSiapButton.onClick.AddListener(BelumSiap);

        backToMenuButton.onClick.AddListener(FindObjectOfType<MaterialController>().backToMenu.GetComponent<Button>().onClick.Invoke);
        lanjutSubmateri.onClick.AddListener(FindObjectOfType<MaterialController>().nextSubmateri.GetComponent<Button>().onClick.Invoke);
        ulangiQuizButton.onClick.AddListener(UlangiQuiz);

    }

    private void Start()
    {
        AppData.instance.PrepareQuiz();
    }

    private void OnEnable()
    {
        readyPanel.gameObject.SetActive(true);
        quizPanel.gameObject.SetActive(false);
        scorePanel.gameObject.SetActive(false);
        timerHabisNotifikasi.SetActive(false);
    }

    Coroutine CR_Timer;
    public void Siap()
    {
        readyPanel.gameObject.SetActive(false);
        quizPanel.gameObject.SetActive(true);
        scorePanel.gameObject.SetActive(false);
        CR_Timer =  StartCoroutine(StartQuiz(AppData.instance.quizzes));
        flagQuizDone = false;
        StartCoroutine(IE_StartTimer());
    }

    public IEnumerator IE_StartTimer()
    {
        float counter = timer;
        while(counter > 0)
        {
            TimeSpan tp = TimeSpan.FromSeconds(counter);
            string zero = tp.Seconds < 10 ? "0" : "";
            TMPQUizTimer.text = $"{tp.Minutes}:{zero}{tp.Seconds}";
            yield return new WaitForSeconds(1);
            if (flagQuizDone)
            {
                yield break;
            }
            counter--;  
        }
        if(CR_Timer != null)
        {
            StopCoroutine(CR_Timer);
            timerHabisNotifikasi.SetActive(true);

            ShowScore();

        }

    }

    public void BelumSiap()
    {
        FindObjectOfType<MaterialController>().PrevSlide();
    }

    public void UlangiQuiz()
    {
        readyPanel.gameObject.SetActive(true);
        quizPanel.gameObject.SetActive(false);
        scorePanel.gameObject.SetActive(false);


    }

    float correct = 0;

    public IEnumerator StartQuiz(QuizData quizData)
    {
        correct = 0;
        foreach (var quiz in quizData.questions)
        {

            TMPQuestion.text = quiz.questionText;
            int correctAnswerIndex = quiz.correctOptionId;
            correctAnswer = correctAnswerIndex;
            
            for (int i = 0; i < buttons.Count; i++)
            {
                if(i >= quiz.options.Count)
                {
                    buttons[i].gameObject.SetActive(false);
                }
                else
                {
                    buttons[i].gameObject.SetActive(true);
                    int index = i;
                    buttons[i].interactable = true;
                    buttons[i].onClick.AddListener(() => SetButton(index));
                    buttons[i].GetComponentInChildren<TMP_Text>().text = quiz.options[i].optionText;
                }
                
            }
            buttons.ForEach(x => x.gameObject.SetActive(true));

            yield return new WaitUntil(() => choice != -1);

            bool isCorrect = choice == correctAnswer;

            if (isCorrect)
            {
                correct++;
            }


            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].interactable = false;
                buttons[i].onClick.RemoveAllListeners();
                buttons[i].GetComponent<Image>().color = i == correctAnswer ? Color.green : Color.red;
            }
            //Play Video;
            yield return new WaitForSeconds(3);


            //RESET
            buttons.ForEach(x => x.gameObject.SetActive(false));
            buttons.ForEach(x => x.GetComponent<Image>().color = Color.white);

            choice = -1;
        }

        ShowScore();


        yield return null;
    }
    public void ShowScore()
    {
        flagQuizDone = true;
        buttons.ForEach(x => x.gameObject.SetActive(false));
        buttons.ForEach(x => x.GetComponent<Image>().color = Color.white);
        quizPanel.SetActive(false);
        scorePanel.SetActive(true);
        AppData.instance.quizzes.score = (correct / (float)AppData.instance.quizzes.questions.Count) * 100;
        TMPScore.text = $"Score :  \n{AppData.instance.quizzes.score.ToString("0.0")}";
        ProgressHandler.instance.SaveData();
    }
    public void SetButton(int index)
    {
        choice = index;
    }


}
