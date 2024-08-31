using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinListManager : MonoBehaviour
{
    [SerializeField] private SkinListItem skinListItemPrefab;
    [SerializeField] private GameObject ballSkinContainer;
    [SerializeField] private GameObject lineSkinContainer;
    [SerializeField] private RectTransform skinListContainer;

    private SkinManager skinManager;
    private List<SkinListItem> ballSkinListItems = new List<SkinListItem>();
    private List<SkinListItem> lineSkinListItems = new List<SkinListItem>();

    private void Awake()
    {
        skinManager = FindObjectOfType<SkinManager>();
        AddBallSkins();
        AddLineSkins();

        foreach (SkinListItem item in ballSkinListItems)
        {
            item.button.onClick.AddListener(() =>
            {
                AudioManager.Instance.Click();
                if (!skinManager.IsBallSkinUnlocked(item.BallSkin)) return;
                skinManager.SetSelectedBallSkin(item.BallSkin);
                HandleBallSkins();
            });
        }
        foreach (SkinListItem item in lineSkinListItems)
        {
            item.button.onClick.AddListener(() =>
            {
                AudioManager.Instance.Click();
                if (!skinManager.IsLineSkinUnlocked(item.LineSkin)) return;
                skinManager.SetSelectedLineSkin(item.LineSkin);
                HandleLineSkins();
            });
        }
    }


    private void AddBallSkins()
    {
        for (int i = 0; i < skinManager.NumberOfBallSkins; i++)
        {
            SkinListItem listItem = Instantiate(skinListItemPrefab);
            listItem.transform.SetParent(ballSkinContainer.transform);
            listItem.transform.localScale = Vector3.one;
            listItem.GetComponent<RectTransform>().SetAsLastSibling();
            listItem.BallSkin = skinManager.GetBallSkinByIndex(i);
            ballSkinListItems.Add(listItem);

            LayoutRebuilder.ForceRebuildLayoutImmediate(skinListContainer);

        }
    }
    private void AddLineSkins()
    {
        for (int i = 0; i < skinManager.NumberOfLineSkins; i++)
        {
            SkinListItem listItem = Instantiate(skinListItemPrefab);
            listItem.transform.SetParent(lineSkinContainer.transform);
            listItem.transform.localScale = Vector3.one;
            listItem.GetComponent<RectTransform>().SetAsLastSibling();
            listItem.LineSkin = skinManager.GetLineSkinByIndex(i);
            lineSkinListItems.Add(listItem);

            LayoutRebuilder.ForceRebuildLayoutImmediate(skinListContainer);
        }
    }



    public void HandleBallSkins()
    {
        BallSkin selectedBallSkin = skinManager.GetSelectedBallSkin();

        foreach (SkinListItem item in ballSkinListItems)
        {
            item.SetUnlocked(skinManager.IsBallSkinUnlocked(item.BallSkin));
            item.SetSelected(item.BallSkin == selectedBallSkin);
        }
    }
    public void HandleLineSkins()
    {
        LineSkin selectedLineSkin = skinManager.GetSelectedLineSkin();

        foreach (SkinListItem item in lineSkinListItems)
        {
            item.SetUnlocked(skinManager.IsLineSkinUnlocked(item.LineSkin));
            item.SetSelected(item.LineSkin == selectedLineSkin);
        }
    }



}
