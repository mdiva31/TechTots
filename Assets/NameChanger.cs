using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameChanger : MonoBehaviour
{
    // Start is called before the first frame update
    public enum Type
    {
        MATERI,
        SUBMATERI,
        NAMA,
        VIDEO
    }
    public Type type;
    void OnEnable()
    {

        StartCoroutine(IE_SetName());

    }

    public IEnumerator IE_SetName()
    {
        yield return new WaitUntil(()=> AppData.instance != null);
        yield return new WaitUntil(() => ProfileData.instance != null);

        if (type == Type.NAMA)
        {
            string text = GetComponent<TMP_Text>().text;
            if (text.Contains("[name]")) text = text.Replace("[name]", ProfileData.instance.username);
            if (text.Contains("[nama]")) text = text.Replace("[nama]", ProfileData.instance.username);
            GetComponent<TMP_Text>().text = text;
        }
        else if (type == Type.MATERI)
        {
            string text = GetComponent<TMP_Text>().text;
            if (text.Contains("[materi]")) text = text.Replace("[materi]", AppData.instance.materiName);
            GetComponent<TMP_Text>().text = text;
        }
        else if (type == Type.SUBMATERI)
        {
            string text = GetComponent<TMP_Text>().text;
            if (text.Contains("[submateri]")) text = text.Replace("[submateri]", AppData.instance.subMateriName);
            GetComponent<TMP_Text>().text = text;
        }else if (type == Type.VIDEO)
        {
            string text = GetComponent<TMP_Text>().text;
            if (text.Contains("[video]")) text = text.Replace("[video]", AppData.instance.choosenVid);
            GetComponent<TMP_Text>().text = text;
        }

    }

}

