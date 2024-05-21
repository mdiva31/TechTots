using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AppData;

public class MaterialController : MonoBehaviour
{
    public GameObject canvasLoading;
    public Transform canvas;
    public string subMateriTerpilih;
    public int currentSlide;

    [System.Serializable]
    public class SlideTemplate
    {
        public SlideLayout layout;
        public GameObject prefab;
    }
    public List<SlideTemplate> templates;

    private IEnumerator Start()
    {
        yield return AppData.instance.IE_GetDefault();
        yield return SetupSlide();
    }

    public List<Slide> slides;
    public IEnumerator SetupSlide()
    {
        List<SubMateri> contents = AppData.instance.materi.contents;
        List<AppData.ContentItem> slides = contents.Find(x=>x.nama == subMateriTerpilih).content;

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

    public void UpdateSlide()
    {
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
