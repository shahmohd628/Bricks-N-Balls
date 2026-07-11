using UnityEngine;

public enum ScreenEdge { Left, Right, Top }

[RequireComponent(typeof(BoxCollider2D))]
public class WallPositioner : MonoBehaviour
{
    [SerializeField] private ScreenEdge edge;
    [SerializeField] private float thickness = 0.5f;

    private void Start()
    {
        float halfW = ScreenBounds.HalfWidth;
        float halfH = ScreenBounds.HalfHeight;
        BoxCollider2D col = GetComponent<BoxCollider2D>();

        switch (edge)
        {
            case ScreenEdge.Left:
                transform.position = new Vector3(-halfW - thickness / 2f, 0f, 0f);
                col.size = new Vector2(thickness, halfH * 2f + thickness * 2f);
                break;
            case ScreenEdge.Right:
                transform.position = new Vector3(halfW + thickness / 2f, 0f, 0f);
                col.size = new Vector2(thickness, halfH * 2f + thickness * 2f);
                break;
            case ScreenEdge.Top:
                transform.position = new Vector3(0f, halfH + thickness / 2f, 0f);
                col.size = new Vector2(halfW * 2f + thickness * 2f, thickness);
                break;
        }
    }
}