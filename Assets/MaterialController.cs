using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static AppData;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class MaterialController : MonoBehaviour
{
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
        yield return AppData.instance.IE_GetMateri();
        yield return SetupSlide();
    }
    [Header("Slides")]
    public List<Slide> slides;
    
    public IEnumerator SetupSlide()
    {


        quizButton.SetActive(false);
        backToMenu.SetActive(false);
        ulangiButton.SetActive(false);  
        nextSubmateri.SetActive(false); 

        nextSlideButton.SetActive(true);
        prevSlideButton.SetActive(true);

        currentSlide = 0;

        canvasLoading.SetActive(true);

        List<SubMateri> contents = AppData.instance.materi.contents;
        subMateriTerpilih = AppData.instance.subMateriName;
        List<AppData.ContentItem> slides = contents.Find(x=>x.nama == subMateriTerpilih).content;

        AppData.instance.quizName = contents.Find(x => x.nama == subMateriTerpilih).quizName;
        // Setup Slide


        foreach(var content in slides)
        {
            GameObject slide = Instantiate(templates.Find(x=>x.layout == content.slideLayout).prefab, canvas);
            this.slides.Add(slide.GetComponent<Slide>());
            StartCoroutine(slide.GetComponent<Slide>().SetupSlide(content));

        }

        yield return new WaitUntil(AllMateriReady);

        UpdateSlide();
        canvasLoading.SetActive(false);
    }

    public bool AllMateriReady()
    {
        foreach(var slide in slides)
        {
            if(slide.ready == false)
            {
                return false;
            }
        }
        return true;

    }

    public void NextSlide()
    {
        currentSlide++;
        UpdateSlide();
    }
    public void PrevSlide()
    {
        currentSlide--;
        UpdateSlide();

    }

    public void RestartSlide()
    {
        currentSlide = 0;
        UpdateSlide();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void NextSubmateri()
    {
        List<SubMateri> contents = AppData.instance.materi.contents;
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
        if (slides[currentSlide].layout == SlideLayout.quiz) 
        {
            quizButton.SetActive(false);
            backToMenu.SetActive(false);
            ulangiButton.SetActive(false);
            nextSubmateri.SetActive(false);
            nextSlideButton.SetActive(false);
            prevSlideButton.SetActive(false);

            List<SubMateri> contents = AppData.instance.materi.contents;
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
            
            quizButton.SetActive(true);
            ulangiButton.SetActive(true);
            List<SubMateri> contents = AppData.instance.materi.contents;
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
        else if (currentSlide == slides.Count-1)
        {
            ulangiButton.SetActive(true);

            List<SubMateri> contents = AppData.instance.materi.contents;
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
