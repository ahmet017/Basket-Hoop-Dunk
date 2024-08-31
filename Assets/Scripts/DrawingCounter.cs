using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DrawingCounter : MonoBehaviour
{
    [SerializeField] private Image iconPrefab;
    public Color fillColor, emptyColor;
    public Color FillColor { get { return fillColor; } set { fillColor = value; } }
    public Color EmptyColor { get { return emptyColor; } set { emptyColor = value; } }

    public UnityEvent onDrawingsOver = new UnityEvent();
    public int numberOfDrawings;
    public int drawingsUsed;
    private List<Image> icons = new List<Image>();

    public void BuildIcons(int count)
    {
        icons.Clear();
        RemoveAllIcons();
        for (int i = 0; i < count; i++)
        {
            Image icon = Instantiate(iconPrefab, this.transform);
            icon.color = fillColor;
            icons.Add(icon);
        }
        numberOfDrawings = count;
        drawingsUsed = 0;
    }
    public void DrawingUsed()
    {
        drawingsUsed++;
        icons[numberOfDrawings - drawingsUsed].color = emptyColor;
        
        if (drawingsUsed == numberOfDrawings)
        {
            onDrawingsOver.Invoke();
        }
    }

    private void RemoveAllIcons()
    {
        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);
    }
}
