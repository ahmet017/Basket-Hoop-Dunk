using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinListItem : MonoBehaviour
{
    public SkinType skinType;
    [SerializeField] private Image icon;
    [SerializeField] private Image mask;
    [SerializeField] private Image circle;
    [SerializeField] private Sprite lineSprite;
    public Button button;
    public Color normalColor = Color.white;
    public Color selectedColor = new Color(0.54f, 0.54f, 0.54f, 1);
    public Color NormalColor { get { return normalColor; } set { normalColor = value; } }
    public Color SelectedColor { get { return selectedColor; } set { selectedColor = value; } }
    public Color MaskColor
    {
        set
        {
            mask.color = value;
        }
    }

    private BallSkin ballSkin;
    public BallSkin BallSkin
    {
        get { return ballSkin; }
        set
        {
            if (value != null)
            {
                ballSkin = value;
                icon.sprite = ballSkin.sprite;
                mask.sprite = ballSkin.sprite;
                skinType = SkinType.BallSkin;
            }
        }
    }
    private LineSkin lineSkin;
    public LineSkin LineSkin
    {
        get { return lineSkin; }
        set
        {
            if (value != null)
            {
                lineSkin = value;
                icon.sprite = lineSprite;
                icon.color = lineSkin.color;
                mask.sprite = lineSprite;
                skinType = SkinType.LineSkin;
            }
        }
    }
    
    private bool unlocked;

    public void SetSelected(bool selected)
    {
        circle.color = selected ? selectedColor : normalColor;
    }
    public void SetUnlocked(bool unlocked)
    {
        this.unlocked = unlocked;
        mask.gameObject.SetActive(!unlocked);
    }
}
