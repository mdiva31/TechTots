using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Dates;
using UnityEngine;

public class ProfileInputController : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField namaField;
    public DatePicker datePicker;
    public TMP_Text feedbackError;

    [Header("Settings")]

    // Aktifkan jika ingin memeriksa saat mulai permainan
    public bool checkOnStart;

    // Aktifkan jika ingin menutup canvas jika data sudah berhasil di update
    public bool closeOnSave;
    public GameObject menuUtama;
    private void Start()
    {

        // Jika benar, maka saat Start dipanggil game akan memeriksa apakah username
        // sudah ada atau belum
        if (checkOnStart)
        {
            // PlayerPrefs.HasKey("Username") berguna untuk memeriksa 
            // LocalStorage apakah ada data yang disimpan dengan key "username"
            if (PlayerPrefs.HasKey("username"))
            {
                menuUtama.SetActive(true);

                gameObject.SetActive(false);
            }
        }  
    }

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("username"))
        {
            // Setup UI jika data sudah ada
            
            feedbackError.text= string.Empty;
            namaField.text = PlayerPrefs.GetString("username");
            SerializableDate date = new SerializableDate();
            date.Date = DateTime.Parse(PlayerPrefs.GetString("tanggalLahir"));
            datePicker.SelectedDate = date;
            
        }
    }

    

    //Menyimpan Data
    public void SaveData()
    {
        bool flagError = false;
        string error = "";
        if (namaField.text == "")
        {
            flagError = true;
            error += "* Nama tidak boleh kosong";
        }
            
        if(datePicker.SelectedDate.Date.ToString() == "")
        {
            flagError = true;
            if(error != "") error += "\n";
            error += "* Tanggal Lahir tidak boleh kosong";

        }
        
        feedbackError.text = error;
        if (flagError)
        {
            feedbackError.color = Color.red;    
            return;
        }
        else
        {
            feedbackError.color = Color.green;

        }


      
        // Menyimpan ke Instance Profile Data yang akan konsisten-
        // didalam semua scene
        ProfileData.instance.SaveData(namaField.text, datePicker.SelectedDate.Date.ToString());
        if (closeOnSave)
        {
            menuUtama.SetActive(true);

            gameObject.SetActive(false);

        }

    }

}
