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
        Screen.orientation = ScreenOrientation.Portrait;
        bgmSlider.onValueChanged.AddListener(SetBGMVoulme);
        yield return new WaitUntil(() => BGMController.instance != null);
        bgmSlider.value = BGMController.instance.bgmSource.volume;
    }

    public void SetBGMVoulme(float value)
    {
        BGMController.instance.bgmSource.volume = value;
        BGMController.instance.SaveVolume();
    }
    public void SetQuizOnly(bool flag)
    {
        AppData.instance.quizOnly = flag;
    }
}
