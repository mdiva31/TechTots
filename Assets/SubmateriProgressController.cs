using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmateriProgressController : MonoBehaviour
{
    public Transform scrollContainer;
    public GameObject prefabButton;
    public bool quiz;

    //Dipanggil saat gameobject active
    private void OnEnable()
    {

        foreach (Transform t in scrollContainer)
        {
            Destroy(t.gameObject);
        }

        // Sama sepserti MateriProgressController.cs
        if (quiz)
        {
            //
            SetupSubmateriQuizProgress();
        }
        else
        {
            SetupSubmateriProgress();
        }
    }

    public void SetupSubmateriProgress()
    {

        // Mendapatkan data data materi dan kontennya 
        var materi = ProgressHandler.instance.progressList.Find(x => x.materi.nama_materi == AppData.instance.materiName).materi;

        foreach (var item in materi.contents)
        {
            // Spawn button button per submateri yang terpilih
            GameObject but = Instantiate(prefabButton, scrollContainer);
            var namaSubmateri = item.nama;
            // Mengkonversikan progress decimal menjadi persentase
            int progress = (int)(item.progress * 100);

            // Assing nama submateri dan progressnya 
            but.GetComponent<ProgressButton>().nama.text = namaSubmateri;
            but.GetComponent<ProgressButton>().progress.text = progress.ToString() + "%";
        }
    }
    public void SetupSubmateriQuizProgress()
    {
        // Mendapatkan data data materi dan quiznya

        var materi = ProgressHandler.instance.progressList.Find(x => x.materi.nama_materi == AppData.instance.materiName).materi;
        var quizzes = ProgressHandler.instance.quizProgressList;

        foreach (var item in materi.contents)
        {
            // Spawn button button per submateri yang terpilih

            GameObject but = Instantiate(prefabButton, scrollContainer);
            var namaSubmateri = item.nama;
            QuizController.QuizData quizData = quizzes.Find(x => x.quizId == item.quizName);
            int progress = (int)quizData.score;

            // Assing nama submateri dan progressnya 
            but.GetComponent<ProgressButton>().nama.text = namaSubmateri;
            but.GetComponent<ProgressButton>().progress.text = progress.ToString();
        }
    }

}
