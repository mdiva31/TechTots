using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    public TMP_InputField inputField;
    private void Start()
    {
        // Mengambil data Notes yang sudah tersimpan pada PlayerPrefs
        // Atau local storage

        if (PlayerPrefs.HasKey("Notes"))
        {
            //Jika ada maka isi InputField dengan string tersebut
            inputField.text = PlayerPrefs.GetString("Notes");
        }
        else
        {
            //Jika belum buatkan Key nya dengan value kosong
            PlayerPrefs.SetString("Notes" , "");
        }

        // Assing Input Field untuk memanggil function SaveNotes setiap -
        // selesai mengedit konten dari Input Field
        inputField.onEndEdit.AddListener(SaveNotes);
    }

    public void SaveNotes(string value)
    {
        PlayerPrefs.SetString("Notes", value);
    }


}
