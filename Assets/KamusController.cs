
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static AppData;
using UnityEngine.Networking;
using UnityEngine.UI;

public class KamusController : MonoBehaviour
{
    // Start is called before the first frame update

    [System.Serializable]
    public class KamusContent
    {
        public string key;
        public string value;
    }

    public Transform scrollContainer;
    public GameObject prefabButton;
    public TMP_Text title;
    public TMP_Text content;
    public GameObject contentPanel;
    public GameObject buttonsPanel;

    public Dictionary<string, string> contents;
    public List<KamusContent> contentList;
    public List<GameObject> buttonsList;    
    private void OnEnable()
    {
        foreach(Transform t in scrollContainer)
        {
            Destroy(t.gameObject); 
        }
        GetContent();
        
    }

    public void GetContent()
    {
        string url = $"https://techtots-d72d3-default-rtdb.asia-southeast1.firebasedatabase.app/Kamus.json?auth=kVcKAXRA3jJtMyuzD8vOYNGZljbi4DljYDSkQ93i";
         StartCoroutine( IE_GetMateri(url));
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
                foreach(KeyValuePair<string, string> pair in contents)
                {
                    GameObject button = Instantiate(prefabButton,scrollContainer);
                    button.GetComponentInChildren<TMP_Text>().text = pair.Key;
                    button.GetComponent<Button>().onClick.AddListener(() => buttonsPanel.SetActive(false));
                    button.GetComponent<Button>().onClick.AddListener(()=>contentPanel.SetActive(true));
                    string val = pair.Value;
                    button.GetComponent<Button>().onClick.AddListener(() => title.text = pair.Key);
                    button.GetComponent<Button>().onClick.AddListener(() => content.text = val);
                    buttonsList.Add(button);
                }
            }
        }
    }

    public void Search(string key)
    {
        if(key != "")
        {
            buttonsList.ForEach(x => x.SetActive(false));
            buttonsList.FindAll(x => x.GetComponentInChildren<TMP_Text>().text.ToLower().Contains(key)).ForEach(  x=>x.SetActive(true));
        }
        else
        {
            buttonsList.ForEach(x => x.SetActive(true));

        }

    }

}
