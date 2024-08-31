using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckInternet : MonoBehaviour
{
    public GameManager GameManager;
    public GameObject NoInternetScreen;
    private float WaitTime = 3;
    void Start()
    {
        StartCoroutine(InternetCheckerwaiter());
    }

    IEnumerator InternetCheckerwaiter()
    {
        yield return new WaitForSeconds(WaitTime);
        InternetChecker();
        StartCoroutine(InternetCheckerwaiter());
    }
    
    public void InternetChecker()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            NoInternetScreen.SetActive(true);
            GameManager.Pause();
            WaitTime = 0.1f;    
        }

        else
        {
            NoInternetScreen.SetActive(false);
            WaitTime = 3f;
        }
    }
}
