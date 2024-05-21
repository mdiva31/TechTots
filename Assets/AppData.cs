using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public enum SlideLayout
{
    title_only,
    title_content,
    title_image,
    image_kiri_text_kanan,
    image_kanan_text_kiri,
    image_only,
    image_subtitle,
    text_only,
    column_2_text_only
,

}
public class AppData : MonoBehaviour
{
    
    public static AppData instance;
    [Header("CONNECTION")]
    [SerializeField] string AuthCode;
    public string materiUrl = "https://techtots-d72d3-default-rtdb.asia-southeast1.firebasedatabase.app/Materi";
    public string kuisUrl;
    [Header("Debug")]
    [TextArea(7,7)]
    public string getMateriUrl;
    public string materiName;
    [System.Serializable]
    public class Materi
    {
        public string materi_icon ;
        public string nama_materi ;
        public List<Dictionary<string, List<ContentItem>>> sub_materi;
        public List<SubMateri> contents = new List<SubMateri>();
    }
    [System.Serializable]

    public class SubMateri
    {
        public string nama ;
        public List<ContentItem> content ;
    }
    [System.Serializable]

    public class ContentItem
    {
        [Header("Single Layout")]

        public string content ;
        public string imageUrl;
        public string image;


        public SlideLayout slideLayout;
        public string slide_layout { 
            get { return slideLayout.ToString(); }     
            set 
            {
                Enum.TryParse(value, out SlideLayout myStatus);
                slideLayout = myStatus;
            }
        }
        public string title ;
        [Header("Multiple Layout")]
        public string content1;
        public string content2;
        public string content3;
        public string image1;
        public string image2;
        public string image3;

    }


    public Materi materi;
    private void Awake()
    {
        if (instance != null)
        {
           
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [ContextMenu("Get Materi")]
    public void GetMateri()
    {
        string url = $"https://techtots-d72d3-default-rtdb.asia-southeast1.firebasedatabase.app/Materi.json?auth=kVcKAXRA3jJtMyuzD8vOYNGZljbi4DljYDSkQ93i&orderBy=\"nama_materi\"&equalTo=\"{materiName}\"";
        StartCoroutine(IE_GetMateri(url));
    }

    public IEnumerator IE_GetDefault()
    {
        string url = $"https://techtots-d72d3-default-rtdb.asia-southeast1.firebasedatabase.app/Materi.json?auth=kVcKAXRA3jJtMyuzD8vOYNGZljbi4DljYDSkQ93i&orderBy=\"nama_materi\"&equalTo=\"{materiName}\"";
        yield return IE_GetMateri(url);
    }
    private IEnumerator IE_GetMateri(string url)
    {
        Debug.Log(url);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Send the request and wait for a response
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error fetching JSON data: " + webRequest.error);
            }
            else
            {
                // Parse JSON data
                string jsonData = webRequest.downloadHandler.text;
                Debug.Log(jsonData);
                Dictionary<string,Materi> dictMateri = JsonConvert.DeserializeObject<Dictionary<string,Materi>>(jsonData);
                materi = dictMateri["0"];
                foreach(var dict in materi.sub_materi)
                {
                    foreach(var key in dict.Keys)
                    {
                        materi.contents.Add(new SubMateri() { nama = key, content = dict[key] });
                    }
                }
            }
        }
    }


}
