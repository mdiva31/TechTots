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
        //Menunggu Progress yang tersimpan sudah ready atau belum

        yield return new WaitUntil(() => ProgressHandler.instance != null);
        yield return new WaitUntil(() => ProgressHandler.instance.progressList != null);

        var materiProgress = ProgressHandler.instance.progressList;
        

        // Spawn baris baris menggunakan pengulangan Foreach dari Progress List yang sudah tersimpan
        foreach (var item in materiProgress)
        {
            GameObject but = Instantiate(prefabButton, scrollContainer);
            var materi = item.materi;
            but.GetComponentInChildren<TMP_Text>().text = materi.nama_materi;
            
            // Semua row dan button yang sudah di spawn akan diberikan listener saat di klik
            // Jadi jika button/row tersebut di klik dia akan memanggil function OpenSubMateri
            // OpenSubMateri tersebut memiliki parameter kelas Materi yang sudah ada isi nya
            but.GetComponent<Button>().onClick.AddListener(() => OpenSubMateri(materi));

        }
    }

    public void OpenSubMateri(AppData.Materi materi)
    {
        // Setup AppData nya 
        // Bertujuan untuk mencatat nama materi saat dibawa ke scene lain
        // Sehingga scene lain dapat menentukan materi mana yang akan ditampilkan
 
        AppData.instance.materiName = materi.nama_materi;

        //Mengaktifkan Panel SubMateri
        subMateriPanel.gameObject.SetActive(true);
    }

}
