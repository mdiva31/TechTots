using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static AppData;

public class MaterialController : MonoBehaviour
{
    [Header("Progress")]
    public float maxProgress;
    public float currentProgress;
    [Header("UI")]
    public GameObject canvasLoading;
    public Transform canvas;
    public string subMateriTerpilih;
    public int currentSlide;
    [Header("Control")]
    public GameObject quizButton;
    public GameObject ulangiButton;
    public GameObject nextSubmateri;
    public GameObject backToMenu;
    public GameObject nextSlideButton;
    public GameObject prevSlideButton;
    [System.Serializable]
    public class SlideTemplate
    {
        public SlideLayout layout;
        public GameObject prefab;
    }
    public List<SlideTemplate> templates;

    private IEnumerator Start()
    {
        // Mengubah orientasi layar menjadi Landscape
        Screen.orientation = ScreenOrientation.LandscapeRight;

        // Menunggu instance dari ProgressHandler untuk tidka sama dengan null
        yield return new WaitUntil(()=> ProgressHandler.instance != null);
        yield return new WaitUntil(() => ProgressHandler.instance.progressList.Count != 0);

        yield return SetupSlide();
    }
    [Header("Slides")]
    public List<Slide> slides;
    
    public IEnumerator SetupSlide()
    {

        // Mematikan panel panel yang tidak dibutuhkan
        quizButton.SetActive(false);
        backToMenu.SetActive(false);
        ulangiButton.SetActive(false);  
        nextSubmateri.SetActive(false); 

        nextSlideButton.SetActive(true);
        prevSlideButton.SetActive(true);

        currentSlide = 0;
        currentProgress = 0;
        canvasLoading.SetActive(true);

        // Mengambil data konten dari AppData 
        AppData.instance.activeMateri = ProgressHandler.instance.progressList.Find(x => x.materi.nama_materi == AppData.instance.materiName).materi;
       
        //Mengabil konten submateri
        List<SubMateri> contents = AppData.instance.activeMateri.contents;
        subMateriTerpilih = AppData.instance.subMateriName;
        List<AppData.ContentItem> slides = contents.Find(x=>x.nama == subMateriTerpilih).content;
        maxProgress = slides.Count-2;
        AppData.instance.quizName = contents.Find(x => x.nama == subMateriTerpilih).quizName;

        //  Setup jika hanya Quiz
        if (AppData.instance.quizOnly)
        {
            //Hanya mengambil data kuis
            slides = slides.FindAll(x => x.slideLayout == SlideLayout.quiz);
        }

        // Spawn semua slide yang sudah disipapkan
        // Untuk semua layout slide sudah ada Templatenya dan dapat dilihat pada-
        // Inpsector -> MaterialController -> Tempalates
        foreach(var content in slides)
        {
            //Slide di spawn menurut slide_layout dan template yang sudah disiapkan.
            GameObject slide = Instantiate(templates.Find(x=>x.layout == content.slideLayout).prefab, canvas);
            this.slides.Add(slide.GetComponent<Slide>());

            //Donwload content yang ada pada slide seperti image
            StartCoroutine(slide.GetComponent<Slide>().SetupSlide(content));

        }
        // Menunggu semua file sudah terdownload
        // Untuk tau materi sudah ready, Program akan memanggil AllMateriReady()
        yield return new WaitUntil(AllMateriReady);

        UpdateSlide();
        canvasLoading.SetActive(false);
    }

    public bool AllMateriReady()
    {
        //Memeriksa apakah masing masing slide sudah ready
        // Jika belum maka akan return false;
        // Jika semua slide sudah ready maka akan return true;
        foreach(var slide in slides)
        {
         
            if(slide.ready == false)
            {
                return false;
            }
        }
        return true;

    }

    //Function ini dipanggil dar OnClick button Next pada CanvasControl
    public void NextSlide()
    {
        currentSlide++;
        UpdateSlide();
    }

    //Function ini dipanggil dar OnClick button Prev pada CanvasControl

    public void PrevSlide()
    {
        if(currentSlide !=0)
            currentSlide--;
        else
            BackToMenu();
        UpdateSlide();

    }

        //Function ini dipanggil dar OnClick Button-Ulangi pada CanvasControl

    public void RestartSlide()
    {
        currentSlide = 0;
        UpdateSlide();
    }

    //Function ini dipanggil dar OnClick Button-Kembali ke Menu pada CanvasControl

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    //Function ini dipanggil dar OnClick Button-Next Submateri ke Menu pada CanvasControl

    public void NextSubmateri()
    {
        List<SubMateri> contents = AppData.instance.activeMateri.contents;
        SubMateri choosenContent = contents.Find(x => x.nama == subMateriTerpilih);

        if (contents.IndexOf(choosenContent) == contents.Count - 1)
        {
            backToMenu.SetActive(true);
        }
        else
        {
            AppData.instance.subMateriName = contents[contents.IndexOf(choosenContent) + 1].nama;
            foreach(var t in slides)
            {
                Destroy(t.gameObject);
            }
            slides.Clear();
            StartCoroutine(SetupSlide());
        }
    }

    public void UpdateSlide()
    {
        // Jika slide kini lebih besar dari progress yang sudah tercatat
        // Maka udpate progress
        // Tujuannya agar setiap mengulang, progress dari slide tidak berkurang
        if(currentSlide > currentProgress)
        {
            currentProgress = currentSlide;
            Materi mat = ProgressHandler.instance.progressList.Find(x => x.materi.nama_materi == AppData.instance.materiName).materi;
            SubMateri subMat = mat.contents.Find(x => x.nama == AppData.instance.subMateriName);
            subMat.progress = Mathf.Clamp(currentProgress / maxProgress,0,1);
            ProgressHandler.instance.SaveData();

        }

        //Jika slide selanjutnya adalah slide quiz
        if (slides[currentSlide].layout == SlideLayout.quiz) 
        {
            // Mematikan UI yang tidak diperlukan
            quizButton.SetActive(false);
            backToMenu.SetActive(false);
            ulangiButton.SetActive(false);
            nextSubmateri.SetActive(false);
            nextSlideButton.SetActive(false);
            prevSlideButton.SetActive(false);

            // Memeriksa apakah submateri yang aktif sekarang adalah Submateri terakhir atau tidak
            List<SubMateri> contents = AppData.instance.activeMateri.contents;
            SubMateri choosenContent = contents.Find(x => x.nama == subMateriTerpilih);

            if (contents.IndexOf(choosenContent) == contents.Count - 1)
            {
                slides[currentSlide].GetComponent<QuizController>().backToMenuButton.gameObject.SetActive(true);
            }
            else
            {
                slides[currentSlide].GetComponent<QuizController>().lanjutSubmateri.gameObject.SetActive(true);
            }
        }
        else if (currentSlide+1 != slides.Count && slides[currentSlide+1].layout == SlideLayout.quiz)
        {
            // Setelah Quiz    
            quizButton.SetActive(true);
            ulangiButton.SetActive(true);
            List<SubMateri> contents = AppData.instance.activeMateri.contents;
            SubMateri choosenContent = contents.Find(x => x.nama == subMateriTerpilih);
            if (contents.IndexOf(choosenContent) == contents.Count -1)
            {
                backToMenu.SetActive(true);
            }
            else
            {
                nextSubmateri.SetActive(true);
            }
            nextSlideButton.gameObject.SetActive(false);
        }   
        // Jika Submateri tidak memiliki quiz, maka masuk kesini
        else if (currentSlide == slides.Count-1)
        {
            
            ulangiButton.SetActive(true);

            List<SubMateri> contents = AppData.instance.activeMateri.contents;
            SubMateri choosenContent = contents.Find(x => x.nama == subMateriTerpilih);

            if (contents.IndexOf(choosenContent) == contents.Count - 1)
            {
                backToMenu.SetActive(true);
            }
            else
            {
                nextSubmateri.SetActive(true);
            }
            nextSlideButton.gameObject.SetActive(false);
        }
        else
        {
            nextSlideButton.gameObject.SetActive(true);
            quizButton.SetActive(false);
            backToMenu.SetActive(false);
            ulangiButton.SetActive(false);
            nextSubmateri.SetActive(false);
        }


        // Mematikan dan menyalakan slide sesuai dengan index currentSlide yang aktif
        foreach (Transform item in canvas)
        {
            Debug.Log(item.name + " index : " + item.GetSiblingIndex());
            if (item.GetSiblingIndex() == currentSlide)
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(false);
            }
        }
        
    }
}
