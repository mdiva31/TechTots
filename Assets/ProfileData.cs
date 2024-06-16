using System.Collections;
using System.Collections.Generic;
using UI.Dates;
using UnityEngine;

public class ProfileData : MonoBehaviour
{
    public static ProfileData instance; 
    public string username;
    public string tanggalLahir;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        if (PlayerPrefs.HasKey("username"))
        {
            username = PlayerPrefs.GetString("username");   
        }
        if (PlayerPrefs.HasKey("tanggalLahir"))
        {
            tanggalLahir = PlayerPrefs.GetString("tanggalLahir");
        }
    }

    // Data akan disimpan menggunakan string yang diterima-
    // Lalu akan disimpan kedalam PLayerPrefs.
    // Tujuannya adalah agar string tersebut selalu tersimpan

    public void SaveData(string newUsername, string newTanggalLahir)
    {
        username = newUsername;
        tanggalLahir = newTanggalLahir;
        PlayerPrefs.SetString("username", username);
        PlayerPrefs.SetString("tanggalLahir",tanggalLahir);
    }


    // Hanya untuk Debugging dimana developer dapat menghapus-
    // data yang sudah tersimpan
    [ContextMenu("Reset Player Pref")]
    public void ResetPlayerPref()
    {
        PlayerPrefs.DeleteKey("username");
        PlayerPrefs.DeleteKey("tanggalLahir");
    }
}
