using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private Line linePrefab;
    [SerializeField] private DrawingCounter drawingCounter;
    [SerializeField] private LayerMask hoopLayer;
    private Line currentLine;
    private Vector2 touchPos;
    private RaycastHit2D hit;
    private Color lineColor;
    private SkinManager skinManager;
    private Vector2 hitPoint = Vector2.zero;

    private void Start()
    {
        skinManager = FindObjectOfType<SkinManager>();
        lineColor = skinManager.GetSelectedLineSkin().color;
    }

    public void SetLineColor(Color color)
    {
        lineColor = color;
    }

    void Update()
    {
        //   if (!GameManager.canDraw || GameManager.paused || EventSystem.current.IsPointerOverGameObject(0)) return;
        if (!GameManager.canDraw || GameManager.paused) return;

        if (Input.GetMouseButtonDown(0))
            BeginDraw();

        if (currentLine != null)
            Draw();

        if (Input.GetMouseButtonUp(0))
            EndDraw();
    }


    private void BeginDraw()
    {
        currentLine = Instantiate(linePrefab, this.transform);
        currentLine.SetLineColor(lineColor);
    }
    private void Draw()
    {
#if UNITY_EDITOR
        touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
#elif UNITY_ANDROID || UNITY_IPHONE                              
        touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
#else
        touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
#endif

        hit = Physics2D.Raycast(touchPos, Vector2.zero, hoopLayer);
        if (hit.collider != null)
            hitPoint = hit.point;

        currentLine.AddPoint(touchPos, hit.collider != null, hitPoint);
    }
    private void EndDraw()
    {
        if (currentLine != null) 
        {
            if (currentLine.pointsCount <= 2)
            {
                Destroy(currentLine.gameObject);
            }
            else
            {
                drawingCounter.DrawingUsed();
            }
            currentLine = null;
        }
    }
    public void DestroyAllLines()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
