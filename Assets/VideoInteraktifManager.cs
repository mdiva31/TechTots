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
        BGMController.instance.bgmSource.volume = 0;

        Screen.orientation = ScreenOrientation.LandscapeRight;

        loadingCanvas.SetActive(true);
        yield return new WaitUntil(() => AppData.instance != null);
        videoUrl = AppData.instance.vidUrl;
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoUrl;
        videoPlayer.Prepare();
        yield return new WaitUntil(()=>videoPlayer.isPrepared == true);

        loadingCanvas.SetActive(false);
        videoPlayer.Play();
    }

    public void BackToMenu()
    {
        BGMController.instance.ResetVolume();

        SceneManager.LoadScene("Menu");

    }


}
