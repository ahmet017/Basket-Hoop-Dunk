using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils;
using UnityUtils.AdmobAdManager;

public class UnlockButton : MonoBehaviour
{
    public GameObject loadingPanel;
    public bool isReady;
    [SerializeField] private TabManager tabManager;
    private Button button;
    private SkinManager skinManager;
    private SkinListManager skinListManager;

    void Awake()
    {
        button = GetComponent<Button>();
        skinManager = FindObjectOfType<SkinManager>();
        skinListManager = FindObjectOfType<SkinListManager>();
        isReady = false;

        button.onClick.AddListener(() =>
        {
            if (!isReady | skinManager.IsAllSkinsUnlocked(tabManager.currentSkinType)) return;

            AdmobAdManager.Instance.ShowRewardedAdIfLoaded((rewarded) => 
            {
                UnityMainThreadDispatcher.Instance.Enqueue(() =>
                {
                    if (tabManager.currentSkinType == SkinType.BallSkin)
                    {
                        skinManager.UnlockRandomBallSkin();
                        skinListManager.HandleBallSkins();
                    }
                    else
                    {
                        skinManager.UnlockRandomLineSkin();
                        skinListManager.HandleLineSkins();
                    }
                    isReady = false;
                    loadingPanel.SetActive(false);
                });
            });
        });
    }

    private void OnEnable()
    {
        AdmobAdManager.Instance.RequestAndLoadRewardedAd();
        StopAllCoroutines();
        StartCoroutine(CheckIfRewardedAdReady());
    }

    private IEnumerator CheckIfRewardedAdReady()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            loadingPanel.SetActive(!AdmobAdManager.Instance.IsRewardedAdReady());
            isReady = AdmobAdManager.Instance.IsRewardedAdReady();
        }
    }
}
