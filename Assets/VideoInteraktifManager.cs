using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoInteraktifManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject loadingCanvas;
    public string videoUrl;
    public VideoPlayer videoPlayer;
    IEnumerator Start()
    {
        //Mematikan Suara dari BGM
        BGMController.instance.bgmSource.volume = 0;

        //Mengatur orientasi dari layar menjadi landscape
        Screen.orientation = ScreenOrientation.LandscapeRight;

        loadingCanvas.SetActive(true);
        yield return new WaitUntil(() => AppData.instance != null);

        //Mengambil data Video URL dari AppData
        videoUrl = AppData.instance.vidUrl;
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoUrl;
        videoPlayer.Prepare();
        yield return new WaitUntil(()=>videoPlayer.isPrepared == true);

        loadingCanvas.SetActive(false);
        // Memutar Video ke GameObjec Video Player
        videoPlayer.Play();
    }

    public void BackToMenu()
    {
        BGMController.instance.ResetVolume();
        SceneManager.LoadScene("Menu");
    }


}
