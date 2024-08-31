using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private PhysicsMaterial2D physicsMat;
    private EdgeCollider2D edgeCollider;
    private float edgeRadius;
    private readonly float minDistance = 0.1f;
    private readonly float maxDistance = 1f;
    private Vector2 lastPoint;
    public int pointsCount = 0;
    private List<Vector2> points = new List<Vector2>();
    private List<Vector2> edges = new List<Vector2>();

    void Awake()
    {
        rigidBody.isKinematic = true;
        edgeRadius = lineRenderer.startWidth / 2;
    }


    public void AddPoint(Vector2 newPoint, bool onHoop, Vector2 hitPoint)
    {
        if (points.Count > 0 && (Vector2.Distance(newPoint, lastPoint) < minDistance || Vector2.Distance(newPoint, lastPoint) > maxDistance))
            return;

        points.Add(newPoint);
        lastPoint = newPoint;
        pointsCount++;
        if (pointsCount > 1)
            AudioManager.Instance.DrawSound(Mathf.Clamp((float)1.5f + (pointsCount * 0.05f), 1.5f, 3f));

        lineRenderer.positionCount = pointsCount;
        lineRenderer.SetPosition(pointsCount - 1, newPoint);

        if (!onHoop)
        {
            if (edges.Count == 0)
            {
                AddEdgeCollider();
            }

            edges.Add(newPoint);
            if (edges.Count > 1)
            {
                edgeCollider.enabled = true;
                edgeCollider.points = edges.ToArray();
            }
        }else
        {
            if (edges.Count > 0)
            {
                edges.Add(hitPoint);
                edgeCollider.points = edges.ToArray();
            }
            edges.Clear();
        }
        
    }
    public void SetLineColor(Color color) 
    { 
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }
    private void AddEdgeCollider()
    {
        edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
        edgeCollider.edgeRadius = edgeRadius;
        edgeCollider.sharedMaterial = physicsMat;
        edgeCollider.enabled = false;
    }



}
