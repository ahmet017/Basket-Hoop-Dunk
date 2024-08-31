using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkinType { BallSkin, LineSkin }

public class SkinManager : MonoBehaviour
{
    [SerializeField] private SkinSettings settings;
    public int NumberOfBallSkins { get { return settings.ballSkins.Count; } }
    public int NumberOfLineSkins { get { return settings.lineSkins.Count; } }
    private LineDrawer lineDrawer;

    void Start()
    {
        lineDrawer = FindObjectOfType<LineDrawer>();
    }


    #region BallSkins
    public void SetSelectedBallSkin(int index)
    {
        PlayerPrefs.SetInt("ballSkin", index);
        PlayerPrefs.Save();
    }
    public void SetSelectedBallSkin(BallSkin ballSkin)
    {
        SetSelectedBallSkin(settings.ballSkins.IndexOf(ballSkin));
    }
    public BallSkin GetSelectedBallSkin()
    {
        int ballSkinIndex = PlayerPrefs.GetInt("ballSkin", 0);

        if (ballSkinIndex < settings.ballSkins.Count)
        {
            return settings.ballSkins[ballSkinIndex];
        }
        else
        {
            return settings.ballSkins[0];
        }
    }
    public List<int> GetUnlockedBallSkins()
    {
        string s = PlayerPrefs.GetString("unlockedBallSkins", "0");
        List<string> stringList = new List<string>(s.Split(","));
        List<int> intList = new List<int>();
        foreach (string str in stringList)
            intList.Add(int.Parse(str));
        return intList;
    }
    public bool IsBallSkinUnlocked(int index)
    {
        if (settings.unlockAllSkins) return true;
        List<int> skins = GetUnlockedBallSkins();
        return skins.Contains(index);
    }
    public bool IsBallSkinUnlocked(BallSkin ballSkin)
    {
        if (settings.unlockAllSkins) return true;
        List<int> skins = GetUnlockedBallSkins();
        return skins.Contains(settings.ballSkins.IndexOf(ballSkin));
    }
    public int UnlockRandomBallSkin()
    {
        List<int> ballSkinsNotUnlocked = new List<int>();
        List<int> unlockedBallSkins = GetUnlockedBallSkins();

        for (int i = 0; i < settings.ballSkins.Count; i++)
        {
            if (!unlockedBallSkins.Contains(i))
            {
                ballSkinsNotUnlocked.Add(i);
            }
        }

        int rand = Random.Range(0, ballSkinsNotUnlocked.Count);
        int randIndex = ballSkinsNotUnlocked[rand];
        unlockedBallSkins.Add(randIndex);

        string s = "";
        string prefix = "";
        foreach (int ballSkinIndex in unlockedBallSkins)
        {
            s += prefix;
            s += ballSkinIndex;
            prefix = ",";
        }
        PlayerPrefs.SetString("unlockedBallSkins", s);
        PlayerPrefs.Save();

        return randIndex;
    }
    public BallSkin GetBallSkinByIndex(int index)
    {
        if (index < NumberOfBallSkins)
            return settings.ballSkins[index];
        return null;
    }
    #endregion



    #region LineSkins
    public void SetSelectedLineSkin(int index)
    {
        lineDrawer.SetLineColor(settings.lineSkins[index].color);
        PlayerPrefs.SetInt("lineSkin", index);
        PlayerPrefs.Save();
    }
    public void SetSelectedLineSkin(LineSkin lineSkin)
    {
        SetSelectedLineSkin(settings.lineSkins.IndexOf(lineSkin));
    }
    public LineSkin GetSelectedLineSkin()
    {
        int lineSkinIndex = PlayerPrefs.GetInt("lineSkin", 0);

        if (lineSkinIndex < settings.lineSkins.Count)
        {
            return settings.lineSkins[lineSkinIndex];
        }
        else
        {
            return settings.lineSkins[0];
        }
    }
    public List<int> GetUnlockedLineSkins()
    {
        string s = PlayerPrefs.GetString("unlockedLineSkins", "0");
        List<string> stringList = new List<string>(s.Split(","));
        List<int> intList = new List<int>();
        foreach (string str in stringList)
            intList.Add(int.Parse(str));
        return intList;
    }
    public bool IsLineSkinUnlocked(int index)
    {
        if (settings.unlockAllSkins) return true;
        List<int> skins = GetUnlockedLineSkins();
        return skins.Contains(index);
    }
    public bool IsLineSkinUnlocked(LineSkin lineSkin)
    {
        if (settings.unlockAllSkins) return true;
        List<int> skins = GetUnlockedLineSkins();
        return skins.Contains(settings.lineSkins.IndexOf(lineSkin));
    }
    public int UnlockRandomLineSkin()
    {
        List<int> lineSkinsNotUnlocked = new List<int>();
        List<int> unlockedLineSkins = GetUnlockedLineSkins();

        for (int i = 0; i < settings.lineSkins.Count; i++)
        {
            if (!unlockedLineSkins.Contains(i))
            {
                lineSkinsNotUnlocked.Add(i);
            }
        }

        int rand = Random.Range(0, lineSkinsNotUnlocked.Count);
        int randIndex = lineSkinsNotUnlocked[rand];
        unlockedLineSkins.Add(randIndex);

        string s = "";
        string prefix = "";
        foreach (int lineSkinIndex in unlockedLineSkins)
        {
            s += prefix;
            s += lineSkinIndex;
            prefix = ",";
        }
        PlayerPrefs.SetString("unlockedLineSkins", s);
        PlayerPrefs.Save();

        return randIndex;
    }
    public LineSkin GetLineSkinByIndex(int index)
    {
        if (index < NumberOfLineSkins)
            return settings.lineSkins[index];
        return null;
    }
    #endregion



    public bool IsAllSkinsUnlocked(SkinType skinType)
    {
        if (settings.unlockAllSkins) return true;
        if (skinType == SkinType.BallSkin)
        {
            return settings.ballSkins.Count == GetUnlockedBallSkins().Count;
        }
        else
        {
            return settings.lineSkins.Count == GetUnlockedLineSkins().Count;
        }
    }

}
