using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider bgmSlider;
    IEnumerator Start()
    {

        Application.targetFrameRate = 60;
        //Mengubah Orientasi permainan setiap masuk kedalam menu
        Screen.orientation = ScreenOrientation.Portrait;
        //Menginisiasi Slider BGM untuk Set BGM Bolume setiap Value berubah
        bgmSlider.onValueChanged.AddListener(SetBGMVoulme);
        yield return new WaitUntil(() => BGMController.instance != null);
        bgmSlider.value = BGMController.instance.bgmSource.volume;
    }

    public void SetBGMVoulme(float value)
    {
        // Mengatur Volumen BGM
        BGMController.instance.bgmSource.volume = value;
        BGMController.instance.SaveVolume();
    }
    public void SetQuizOnly(bool flag)
    {
        //Memberikan flag bahwa konten yang ditampilkan hanya konten saja


        AppData.instance.quizOnly = flag;
    }

    public void Keluar()
    {
        Application.Quit();
    }
}
