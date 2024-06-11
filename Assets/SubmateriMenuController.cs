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
       
        foreach (Transform t in scrollContainer)
        {
            Destroy(t.gameObject);
        }
        var materi = ProgressHandler.instance.progressList.Find(x=>x.materi.nama_materi == AppData.instance.materiName).materi;

        foreach (var item in materi.contents)
        {
            GameObject but = Instantiate(prefabButton, scrollContainer);
            var namaSubmateri = item.nama;
            but.GetComponentInChildren<TMP_Text>().text = namaSubmateri;
            but.GetComponent<Button>().onClick.AddListener(() => OpenSubmateri(namaSubmateri));
        }
    }

    public void OpenSubmateri(string submateriName)
    {
        AppData.instance.subMateriName = submateriName;
        SceneManager.LoadScene("SubMateriScene");
    }
}
