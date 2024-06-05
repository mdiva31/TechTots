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
        foreach (Transform t in scrollContainer)
        {
            Destroy(t.gameObject);
        }
        GetContent();

    }

    public void GetContent()
    {
        string url = $"https://techtots-d72d3-default-rtdb.asia-southeast1.firebasedatabase.app/VidInteraktif.json?auth=kVcKAXRA3jJtMyuzD8vOYNGZljbi4DljYDSkQ93i";
        StartCoroutine(IE_GetMateri(url));
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
                contents = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonData);
                foreach (KeyValuePair<string, string> pair in contents)
                {
                    GameObject button = Instantiate(prefabButton, scrollContainer);
                    button.GetComponentInChildren<TMP_Text>().text = pair.Key;
                    string key = pair.Key;
                    string val = pair.Value;
                    button.GetComponent<Button>().onClick.AddListener(() => AppData.instance.choosenVid = key);
                    button.GetComponent<Button>().onClick.AddListener(() => AppData.instance.vidUrl = val);
                    button.GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene("VideoInteraktifScene"));


                }
            }
        }
    }
}
