using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenController : MonoBehaviour
{
    public float splashScreenTimer;
    public GameObject firstSplashScreen;
    public GameObject secondSplashScreen;
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => AppData.instance != null);
        if(AppData.instance.splashScreenDone == true)
        {
            Destroy(gameObject);
            yield break;
        }
        // Mematikan Splash Screen kedua
        firstSplashScreen.SetActive(true);
        secondSplashScreen.SetActive(false);

        // Menunggu untuk beberapa detik sebagai splash screen
        // Durasi dari splash screen bisa di atur via Inspector
        // Pada Variable splashScreenTimer

        yield return new WaitForSeconds(splashScreenTimer);
        firstSplashScreen.SetActive(false);
        secondSplashScreen.SetActive(true);
        AppData.instance.splashScreenDone = true;

    }
}
