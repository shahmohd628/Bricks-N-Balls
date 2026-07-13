using UnityEngine;

public static class ScreenBounds
{
    public static float HalfHeight => Camera.main.orthographicSize;
    public static float HalfWidth => Camera.main.orthographicSize * Camera.main.aspect;
}