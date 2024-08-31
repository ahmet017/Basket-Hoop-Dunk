using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    [SerializeField] private Button ballSkinsButton, lineSkinsButton;
    [SerializeField] private GameObject ballSkinsScreen, lineSkinsScreen;
    public Color tabColor, selectedTabColor;
    public Color TabColor { get { return tabColor; } set { tabColor = value; } }
    public Color SelectedTabColor { get { return selectedTabColor; } set { selectedTabColor = value; } }

    public SkinType currentSkinType;
    private Image ballSkinsTabBackground, lineSkinsTabBackground;
    private Image ballSkinsTabIcon, lineSkinsTabIcon;
    private SkinListManager skinListManager;

    void Start()
    {
        skinListManager = FindObjectOfType<SkinListManager>();
        ballSkinsTabBackground = ballSkinsButton.GetComponent<Image>();
        lineSkinsTabBackground = lineSkinsButton.GetComponent<Image>();
        ballSkinsTabIcon = ballSkinsButton.transform.Find("Icon").GetComponent<Image>();
        lineSkinsTabIcon = lineSkinsButton.transform.Find("Icon").GetComponent<Image>();

        BallSkinsSelected();

        ballSkinsButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.Click();
            if (currentSkinType == SkinType.BallSkin) return;
            BallSkinsSelected();
        });

        lineSkinsButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.Click();
            if (currentSkinType == SkinType.LineSkin) return;
            LineSkinsSelected();
        });
    }

    private void BallSkinsSelected()
    {
        currentSkinType = SkinType.BallSkin;
        ballSkinsTabBackground.color = selectedTabColor;
        lineSkinsTabBackground.color = tabColor;
        ballSkinsTabIcon.color = tabColor;
        lineSkinsTabIcon.color = selectedTabColor;
        ballSkinsScreen.SetActive(true);
        lineSkinsScreen.SetActive(false);
        skinListManager.HandleBallSkins();
        
    }
    private void LineSkinsSelected()
    {
        currentSkinType = SkinType.LineSkin;
        ballSkinsTabBackground.color = tabColor;
        lineSkinsTabBackground.color = selectedTabColor;
        ballSkinsTabIcon.color = selectedTabColor;
        lineSkinsTabIcon.color = tabColor;
        ballSkinsScreen.SetActive(false);
        lineSkinsScreen.SetActive(true);
        skinListManager.HandleLineSkins();
    }
}
