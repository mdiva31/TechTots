
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


    // Dipanggil saat gameobject / panel di aktifkan
    private void OnEnable()
    {
        // Mengahpus baris baris button yang sudah ada-
        // Tujuannya untuk nge reset data jika terjadi perubahan terhadap data kamus
        foreach(Transform t in scrollContainer)
        {
            Destroy(t.gameObject); 
        }

        // CRUD-> Read data dari database untuk mendapatkan kamus
        GetContent();
        
    }

    public void GetContent()
    {
        // Setup URL untuk database.
        string url = $"https://techtots-d72d3-default-rtdb.asia-southeast1.firebasedatabase.app/Kamus.json?auth=kVcKAXRA3jJtMyuzD8vOYNGZljbi4DljYDSkQ93i";
         StartCoroutine( IE_GetMateri(url));
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

            // Jika data error maka tampilkan error seperti ini 
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
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
                foreach(KeyValuePair<string, string> pair in contents)
                {

                    // Spawn button button row tiap kamus
                    GameObject button = Instantiate(prefabButton,scrollContainer);

                    // Memasukan nama Key menjadi nama button nya
                    button.GetComponentInChildren<TMP_Text>().text = pair.Key;

                    // Memberikan listener pada button saat di click. Saat button di click-
                    // maka akan mengaktifkan panel Content dari kamus tersebut
                    button.GetComponent<Button>().onClick.AddListener(() => buttonsPanel.SetActive(false));
                    button.GetComponent<Button>().onClick.AddListener(()=>contentPanel.SetActive(true));
                    string val = pair.Value;

                    // Saat button di click , maka judul dan content nya akan diubah sesuai-
                    // data dari dictionary

                    button.GetComponent<Button>().onClick.AddListener(() => title.text = pair.Key);
                    button.GetComponent<Button>().onClick.AddListener(() => content.text = val);
                    buttonsList.Add(button);
                }
            }
        }
    }

    // Pencarian dipanggil pada GameObject Input Field IF_Search
    // Di panggil saat OnValueChanged
    // Jadi setiap value dari input field berubah, maka function ini akan dipanggil
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
