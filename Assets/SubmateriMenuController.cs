using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SubmateriMenuController : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform scrollContainer;
    public GameObject prefabButton;

    private void OnEnable()
    {
       //Saat gameobject active hapus terlebih dahulu row sebelumnya yang sudah ada
       //Tujuannya agar jika terjadi perubahan content maka data akan berubah.
        foreach (Transform t in scrollContainer)
        {
            Destroy(t.gameObject);
        }

        //Proses pengambilan data materi dan SubMateri
        var materi = ProgressHandler.instance.progressList.Find(x=>x.materi.nama_materi == AppData.instance.materiName).materi;


        //Spawn baris baris button tiap submateri pada materi yang dipilih
        foreach (var item in materi.contents)
        {
            GameObject but = Instantiate(prefabButton, scrollContainer);
            var namaSubmateri = item.nama;
            but.GetComponentInChildren<TMP_Text>().text = namaSubmateri;

            //Setiap baris akan diberikan OnClick Listener untuk membuka OpenSubmateri
            but.GetComponent<Button>().onClick.AddListener(() => OpenSubmateri(namaSubmateri));
        }
    }

    public void OpenSubmateri(string submateriName)
    {
        //Menyimpan nama submateri yang ingin dibuka
        AppData.instance.subMateriName = submateriName;

        //Berpindah scene ke scene SubMateri untuk melihat slide slide nya
        SceneManager.LoadScene("SubMateriScene");
    }
}
