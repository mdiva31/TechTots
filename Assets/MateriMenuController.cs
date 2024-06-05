using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MateriMenuController : MonoBehaviour
{



    public Transform scrollContainer;
    public GameObject prefabButton;
    public GameObject subMateriPanel;
   
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => ProgressHandler.instance != null);
        yield return new WaitUntil(() => ProgressHandler.instance.progressList != null);

        var materiProgress = ProgressHandler.instance.progressList;

        foreach (var item in materiProgress)
        {
            GameObject but = Instantiate(prefabButton, scrollContainer);
            var materi = item.materi;
            but.GetComponentInChildren<TMP_Text>().text = materi.nama_materi;
            but.GetComponent<Button>().onClick.AddListener(() => OpenSubMateri(materi));

        }
    }

    public void OpenSubMateri(AppData.Materi materi)
    {
        AppData.instance.materiName = materi.nama_materi;
        subMateriPanel.gameObject.SetActive(true);
    }

}
