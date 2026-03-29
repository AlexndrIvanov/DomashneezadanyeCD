using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SpringLineRenderer : MonoBehaviour
{
    public Transform anchorPoint;
    public Transform massPoint;

    [Header("Spring Visual Settings")]
    public int segments = 16;
    public float zigzagWidth = 0.1f;
    public float endOffset = 0.12f;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (anchorPoint == null || massPoint == null)
            return;

        DrawSpring();
    }

    private void DrawSpring()
    {
        Vector3 start = anchorPoint.position;
        Vector3 end = massPoint.position;

        Vector3 direction = (end - start).normalized;
        float totalLength = Vector3.Distance(start, end);

        if (totalLength <= 0.01f)
            return;

        Vector3 perpendicular = Vector3.right;

        if (Mathf.Abs(Vector3.Dot(direction, Vector3.right)) > 0.9f)
            perpendicular = Vector3.up;

        Vector3 springStart = start + direction * endOffset;
        Vector3 springEnd = end - direction * endOffset;

        float springLength = Vector3.Distance(springStart, springEnd);

        if (springLength <= 0.01f)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
            return;
        }

        lineRenderer.positionCount = segments + 2;
        lineRenderer.SetPosition(0, start);

        for (int i = 0; i < segments; i++)
        {
            float t = (i + 1f) / (segments + 1f);
            Vector3 point = Vector3.Lerp(springStart, springEnd, t);

            float offsetDirection = (i % 2 == 0) ? 1f : -1f;
            point += perpendicular * zigzagWidth * offsetDirection;

            lineRenderer.SetPosition(i + 1, point);
        }

        lineRenderer.SetPosition(segments + 1, end);
    }
}