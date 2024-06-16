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

        // Flag menentukan apakah ingin melihat progress quiz atau tida
        // Variable Quiz diubah pada Inspector.
        // Pada Panel Progress Materi -> quiz == false;
        // pada Panel Progress Quiz => quiz == true;
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

        //Mendapatkan Progress List
        var progressList = ProgressHandler.instance.progressList;

        foreach (var item in progressList)
        {
            var materi = item.materi;
            //Spawn button button materi
            GameObject but = Instantiate(prefabButton, scrollContainer);
            var namaMateri = materi.nama_materi;
            float submateriCount = materi.contents.Count;
            float currentSubmateriProgress = 0;

            //Mengkalkulasikan progress materi
            foreach (var submateri in materi.contents)
            {
                currentSubmateriProgress += submateri.progress;
            }
            
            int percentage = (int)((currentSubmateriProgress / submateriCount) * 100);


            // Setup Button
            but.GetComponent<ProgressButton>().nama.text = namaMateri;
            but.GetComponent<ProgressButton>().progress.text = percentage.ToString() + "%";

            //Setiap button diberikan listener saat di klik ke function OpenSubmateriProgress-
            //dengan parameter yaitu materi nya.
            but.GetComponent<Button>().onClick.AddListener(() => OpenSubmateriProgress(materi));
        }
    }
    public void SetupMateriQuizProgress()
    {
        //Mendapaktan progress list dan quiz list
        var progressList = ProgressHandler.instance.progressList;
        var quizlist = ProgressHandler.instance.quizProgressList;
        foreach (var item in progressList)
        {
            var materi = item.materi;

            //Spawn button button sesuai jumlah materi
            GameObject but = Instantiate(prefabButton, scrollContainer);
            var namaMateri = materi.nama_materi;
            
            but.GetComponent<ProgressButton>().nama.text = namaMateri;
            //Setiap button diberikan listener saat di klik ke function OpenSubmateriQuizProgress-
            //dengan parameter yaitu materi nya.
            but.GetComponent<Button>().onClick.AddListener(() => OpenSubmateriQuizProgress(materi));
        }
    }

    public void OpenSubmateriProgress(AppData.Materi materi)
    {
        // Mencatat materi apa yang sedang terbuka
        AppData.instance.materiName = materi.nama_materi;

        // Mengaktifkan panel submateri progress
        // Panel yang akan aktif dapat dilihat pada inspector
        submateriProgressPanel.gameObject.SetActive(true);
    }
    public void OpenSubmateriQuizProgress(AppData.Materi materi)
    {
        // Mencatat materi apa yang sedang terbuka
        AppData.instance.materiName = materi.nama_materi;

        // Mengaktifkan panel submateri quiz progress
        // Panel yang akan aktif dapat dilihat pada inspector
        submateriQuizPanel.gameObject.SetActive(true);
    }
}
