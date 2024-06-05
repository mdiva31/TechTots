using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static AppData;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System.Net;

public class ProgressHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [System.Serializable]
    public class MateriProgress
    {
        public AppData.Materi materi;
    }
    [System.Serializable]
    public class SubmateriProgress
    {
        public string name;
        public float progress;
    }




    public string currentVersion;
    private string getVersionString;
    public List<MateriProgress> progressList;
    public List<QuizController.QuizData> quizProgressList;
    public static ProgressHandler instance;

    [Header("URL")]
    string materiUrl = "https://techtots-d72d3-default-rtdb.asia-southeast1.firebasedatabase.app/Materi.json?auth=kVcKAXRA3jJtMyuzD8vOYNGZljbi4DljYDSkQ93i";
    string quizUrl = "https://techtots-d72d3-default-rtdb.asia-southeast1.firebasedatabase.app/Quiz.json?auth=kVcKAXRA3jJtMyuzD8vOYNGZljbi4DljYDSkQ93i";

    IEnumerator Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            yield break;
        }
        instance = this;

        yield return IE_GetVersion();
        bool flagUpdate = false;
        if (PlayerPrefs.HasKey("Version"))
        {
            if (PlayerPrefs.GetString("Version") != getVersionString)
            {
                flagUpdate = true;
            }
        }
        else
        {

            flagUpdate = true;
        }

        if (flagUpdate)
        {

            if (PlayerPrefs.HasKey("MateriProgress"))
            {
                var oldVersion = JsonConvert.DeserializeObject<List<MateriProgress>>(PlayerPrefs.GetString("MateriProgress"));
                var oldQuizVersion = JsonConvert.DeserializeObject<List<QuizController.QuizData>>(PlayerPrefs.GetString("QuizProgress"));

                yield return IE_GetMateri(materiUrl);
                yield return IE_GetQuiz(quizUrl);

                foreach(var prog in oldVersion)
                {
                    var newMateri = progressList.Find(x => x.materi.nama_materi == prog.materi.nama_materi);
                    var oldMateri = prog.materi;

                    if (newMateri == null)
                        continue;
                    
                    foreach(var newSub in newMateri.materi.contents)
                    {

                        //if (oldMateri.contents.Find(x => x.nama == newSub.nama) != null)
                        //{
                        //}
                        //QuizController.QuizData quizData = quizProgressList.Find(x => x.quizId == newSub.quizName);

                        //QuizController.QuizData oldQuizData = oldQuizVersion.Find(x => x.quizId == quizData.quizId);
                        //if(oldQuizData != null)
                        //{
                        //    quizData.score = oldQuizData.score;
                        //}

                    }
                }
                ProgressHandler.instance.SaveData();
            }
            else
            {
                yield return IE_GetMateri(materiUrl);
                yield return IE_GetQuiz(quizUrl);

                SaveData();

                Debug.Log("Saving To Storage...");
            }
        }
        else
        {
            if (!PlayerPrefs.HasKey("MateriProgress"))
            {
                yield return IE_GetMateri(materiUrl);
                SaveMateriProgress();
            }
            if (!PlayerPrefs.HasKey("QuizProgress"))
            {
                yield return IE_GetQuiz(quizUrl);
                SaveQuizProgress();
            }
            Debug.Log("Loading Content...");
            progressList = JsonConvert.DeserializeObject<List<MateriProgress>>(PlayerPrefs.GetString("MateriProgress"));
            quizProgressList = JsonConvert.DeserializeObject<List<QuizController.QuizData>>(PlayerPrefs.GetString("QuizProgress"));
        }




        // Check For Progress
        // Update Version
        PlayerPrefs.SetString("Version", getVersionString);
        currentVersion = PlayerPrefs.GetString("Version");

    }


    public IEnumerator IE_GetVersion()
    {
        Debug.Log("Getting latest version...");
        string url = "https://techtots-d72d3-default-rtdb.asia-southeast1.firebasedatabase.app/Version.json?auth=kVcKAXRA3jJtMyuzD8vOYNGZljbi4DljYDSkQ93i";
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
                getVersionString = jsonData;
                Debug.Log("Current Version : " + jsonData);

            }
        }

    }


    [ContextMenu("Donwload Content")]
    public void GetAllMateri()
    {
        string url = $"https://techtots-d72d3-default-rtdb.asia-southeast1.firebasedatabase.app/Materi.json?auth=kVcKAXRA3jJtMyuzD8vOYNGZljbi4DljYDSkQ93i";

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
                string json = "{ \"materi\" : " + jsonData + "}";
                JObject jObject = JObject.Parse(json);

                foreach(var content in jObject["materi"])
                {
                    Debug.Log(content.ToString());
                    Materi dictMateri = JsonConvert.DeserializeObject< Materi>(content.ToString());
                    var progress = new MateriProgress();
                    progress.materi = dictMateri;
                    foreach (var dict in progress.materi.sub_materi)
                    {
                        foreach (var key in dict.Keys)
                        {
                            progress.materi.contents.Add(new SubMateri() { nama = key, content = dict[key] });
                            if (dict[key][dict[key].Count - 1].slide_layout == "quiz")
                            {
                                progress.materi.contents.Find(x => x.nama == key).quizName = dict[key][dict[key].Count - 1].quizName;
                            }
                        }
                    }
                    progressList.Add(progress);
                }
               
            }
        }
    }
    private IEnumerator IE_GetQuiz(string url)
    {
        using(UnityWebRequest webRequest = UnityWebRequest.Get(url))
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
                Dictionary<string, QuizController.QuizData> quizzes = JsonConvert.DeserializeObject<Dictionary<string, QuizController.QuizData>>(jsonData);
                foreach(var key in quizzes.Keys)
                {
                    QuizController.QuizData quiz = quizzes[key];
                    quiz.quizId = key.ToString();
                    quizProgressList.Add(quiz);
                }
                
            }
        }
    }
    [ContextMenu("Save Data")]
    public void SaveData()
    {
        Debug.Log("Data Saved");
        SaveMateriProgress();
        SaveQuizProgress();
    }
    public void SaveMateriProgress()
    {
        PlayerPrefs.SetString("MateriProgress", JsonConvert.SerializeObject(progressList));
    }
    public void SaveQuizProgress()
    {
        PlayerPrefs.SetString("QuizProgress", JsonConvert.SerializeObject(quizProgressList));
    }
    [ContextMenu("Reset All Data")]
    public void ResetData()
    {
        PlayerPrefs.DeleteAll();

    }
}
