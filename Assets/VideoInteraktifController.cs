using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VideoInteraktifController : MonoBehaviour 
{

    [System.Serializable]
    public class VideoContent
    {
        public string key;
        public string value;
    }

    public Transform scrollContainer;
    public GameObject prefabButton;
    public TMP_Text content;
    public GameObject contentPanel;
    public GameObject buttonsPanel;

    public Dictionary<string, string> contents;
    public List<VideoContent> contentList;
    private void OnEnable()
    {
        // Mengahpus baris baris button yang sudah ada-
        // Tujuannya untuk nge reset data jika terjadi perubahan terhadap data video interaktif

        foreach (Transform t in scrollContainer)
        {
            Destroy(t.gameObject);
        }

        // CRUD-> Read data dari database untuk mendapatkan video interaktif
        GetContent();
    }

    public void GetContent()
    {
        // Setup URL untuk database.

        string url = $"https://techtots-d72d3-default-rtdb.asia-southeast1.firebasedatabase.app/VidInteraktif.json?auth=kVcKAXRA3jJtMyuzD8vOYNGZljbi4DljYDSkQ93i";
        StartCoroutine(IE_GetMateri(url));
    }
    private IEnumerator IE_GetMateri(string url)
    {
        Debug.Log(url);

        // Untuk melakukan operasi pada API kita harus menggunakan UnityWebRequest
        // Karna disini kita ingin mengambil data berarti kita menggunaka function GET
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Mengirim Request dan Menunggu Respon
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                // Jika data error maka tampilkan error seperti ini 

                Debug.LogError("Error fetching JSON data: " + webRequest.error);
            }
            else
            {
                // Parse JSON data
                string jsonData = webRequest.downloadHandler.text;
                Debug.Log(jsonData);

                // Deserializing Object menjadi sebuah Dictionary
                contents = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonData);

                //Setup button sesuai dengan Key dan Valuenya
                foreach (KeyValuePair<string, string> pair in contents)
                {
                    // Spawn button button sesuai pada database.
                    GameObject button = Instantiate(prefabButton, scrollContainer);
                    button.GetComponentInChildren<TMP_Text>().text = pair.Key;
                    string key = pair.Key;
                    string val = pair.Value;

                    // Memberikan listener pada button saat diclick
                    // Pada saat button di click , button akan memanngil event -event berikut

                    // Menulis pada AppData nama dari Video yang terpilih
                    button.GetComponent<Button>().onClick.AddListener(() => AppData.instance.choosenVid = key);
                    
                    // Menulis pada AppData url dari Video yang terpilih
                    button.GetComponent<Button>().onClick.AddListener(() => AppData.instance.vidUrl = val);
                    
                    // Perpindahan scene menuju video interaktif scene
                    button.GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene("VideoInteraktifScene"));


                }
            }
        }
    }
}
