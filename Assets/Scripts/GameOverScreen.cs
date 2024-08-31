using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityUtils;
using UnityUtils.AdmobAdManager;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score, bestScore;

    public int BestScore 
    {
        set
        {
            bestScore.text = value.ToString();
        }
    }
    public int Score
    {
        set
        {
            score.text = value.ToString();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        GameManager.GamesPlayed++;

        if (GameManager.GamesPlayed >= AdmobAdManager.Instance.LevelsToShowInterstitialAd)
        {
            AdmobAdManager.Instance.ShowInterstitialAdIfLoaded((adOpened) => 
            {
                UnityMainThreadDispatcher.Instance.Enqueue(() =>
                {
                    if (adOpened)
                    {
                        GameManager.GamesPlayed = 0;
                    }
                });
            });
        }
    }
}
