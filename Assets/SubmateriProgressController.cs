using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmateriProgressController : MonoBehaviour
{
    public Transform scrollContainer;
    public GameObject prefabButton;
    public bool quiz;
    private void OnEnable()
    {

        foreach (Transform t in scrollContainer)
        {
            Destroy(t.gameObject);
        }
        if (quiz)
        {
            SetupSubmateriQuizProgress();
        }
        else
        {
            SetupSubmateriProgress();
        }
    }

    public void SetupSubmateriProgress()
    {
        var materi = ProgressHandler.instance.progressList.Find(x => x.materi.nama_materi == AppData.instance.materiName).materi;

        foreach (var item in materi.contents)
        {

            GameObject but = Instantiate(prefabButton, scrollContainer);
            var namaSubmateri = item.nama;
            int progress = (int)item.progress * 100;

            but.GetComponent<ProgressButton>().nama.text = namaSubmateri;
            but.GetComponent<ProgressButton>().progress.text = progress.ToString() + "%";
        }
    }
    public void SetupSubmateriQuizProgress()
    {
        var materi = ProgressHandler.instance.progressList.Find(x => x.materi.nama_materi == AppData.instance.materiName).materi;
        var quizzes = ProgressHandler.instance.quizProgressList;

        foreach (var item in materi.contents)
        {

            GameObject but = Instantiate(prefabButton, scrollContainer);
            var namaSubmateri = item.nama;
            QuizController.QuizData quizData = quizzes.Find(x => x.quizId == item.quizName);
            int progress = (int)quizData.score;

            but.GetComponent<ProgressButton>().nama.text = namaSubmateri;
            but.GetComponent<ProgressButton>().progress.text = progress.ToString();
        }
    }

}
