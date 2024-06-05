using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MateriProgressController : MonoBehaviour
{
    public Transform scrollContainer;
    public GameObject prefabButton;
    public GameObject submateriProgressPanel;
    public GameObject submateriQuizPanel;

    public bool quiz;
    private void OnEnable()
    {
        
        foreach (Transform t in scrollContainer)
        {
            Destroy(t.gameObject);
        }
        if (quiz)
        {
            SetupMateriQuizProgress();
        }
        else
        {
            SetupMateriProgress();
        }

    }

    public void SetupMateriProgress()
    {
        var progressList = ProgressHandler.instance.progressList;

        foreach (var item in progressList)
        {
            var materi = item.materi;
            GameObject but = Instantiate(prefabButton, scrollContainer);
            var namaMateri = materi.nama_materi;
            float submateriCount = materi.contents.Count;
            float currentSubmateriProgress = 0;
            foreach (var submateri in materi.contents)
            {
                currentSubmateriProgress += submateri.progress;
            }

            int percentage = (int)((currentSubmateriProgress / submateriCount) * 100);

            but.GetComponent<ProgressButton>().nama.text = namaMateri;
            but.GetComponent<ProgressButton>().progress.text = percentage.ToString() + "%";
            but.GetComponent<Button>().onClick.AddListener(() => OpenSubmateriProgress(materi));
        }
    }
    public void SetupMateriQuizProgress()
    {
        var progressList = ProgressHandler.instance.progressList;
        var quizlist = ProgressHandler.instance.quizProgressList;
        foreach (var item in progressList)
        {
            var materi = item.materi;
            GameObject but = Instantiate(prefabButton, scrollContainer);
            var namaMateri = materi.nama_materi;
            but.GetComponent<ProgressButton>().nama.text = namaMateri;
            but.GetComponent<Button>().onClick.AddListener(() => OpenSubmateriQuizProgress(materi));
        }
    }

    public void OpenSubmateriProgress(AppData.Materi materi)
    {
        AppData.instance.materiName = materi.nama_materi;
        submateriProgressPanel.gameObject.SetActive(true);
    }
    public void OpenSubmateriQuizProgress(AppData.Materi materi)
    {
        AppData.instance.materiName = materi.nama_materi;
        submateriQuizPanel.gameObject.SetActive(true);
    }
}
