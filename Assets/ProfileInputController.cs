using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Dates;
using UnityEngine;

public class ProfileInputController : MonoBehaviour
{
    public TMP_InputField namaField;
    public DatePicker datePicker;
    public TMP_Text feedbackError;
    public bool checkOnStart;
    public bool closeOnSave;
    public GameObject menuUtama;
    private void Start()
    {
        if (checkOnStart)
        {
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
            feedbackError.text= string.Empty;
            namaField.text = PlayerPrefs.GetString("username");
            SerializableDate date = new SerializableDate();
            date.Date = DateTime.Parse(PlayerPrefs.GetString("tanggalLahir"));
            datePicker.SelectedDate = date;
            
        }
    }
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


      
        ProfileData.instance.SaveData(namaField.text, datePicker.SelectedDate.Date.ToString());
        if (closeOnSave)
        {
            menuUtama.SetActive(true);

            gameObject.SetActive(false);

        }

    }

}
