using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLoadingController : MonoBehaviour
{
    // Start is called before the first frame update
    
    IEnumerator Start()
    {
        yield return new WaitUntil(() => ProgressHandler.instance != null);
        yield return new WaitUntil(() => ProgressHandler.instance.progressList != null);
        gameObject.SetActive(false);
    }
  
    
}
