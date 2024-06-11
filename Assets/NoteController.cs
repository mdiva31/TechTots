using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    public TMP_InputField inputField;
    private void Start()
    {
        if (PlayerPrefs.HasKey("Notes"))
        {
            inputField.text = PlayerPrefs.GetString("Notes");
        }
        else
        {
            PlayerPrefs.SetString("Notes" , "");
        }

        inputField.onEndEdit.AddListener(SaveNotes);
    }

    public void SaveNotes(string value)
    {
        PlayerPrefs.SetString("Notes", value);
    }


}
